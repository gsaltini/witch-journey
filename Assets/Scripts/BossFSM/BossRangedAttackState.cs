using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangedAttackState : BossState
{
    float attackTimer, attackAnimationStart, animationTime;
    bool attacked, previousGlow, previousLaser, nextStateSet;
    int endLoopCounter = 0;
    Transform rangedPoint;
    BossEnemy bossEnemy;
    Animator animator;
    public override void Enter(BossStateMachine bossStateMachine)
    {
        bossEnemy = bossStateMachine.bossEnemy;
        rangedPoint = bossEnemy.rangedPoint;
        animator = bossStateMachine.animator;
        attacked = false;
        nextStateSet = false;

        endLoopCounter = endLoopCounter + 1;
        
        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips) {
            if (clip.name == "Laser Cast" && bossStateMachine.previousState == bossStateMachine.chase && bossEnemy.inStageTwo && bossEnemy.bossTwo) {
                attackTimer = clip.length;
            } else if (clip.name == "Ranged Attack") {
                attackTimer = clip.length;
            }
        }

        if (bossEnemy.bossThree) {
            if (bossStateMachine.transform.position.x - bossStateMachine.playerTransform.position.x < 0) {
                bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.back);
            } else {
                bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            }
        }

        animator.SetBool("idle", true);

        Debug.Log(attackTimer);
        Debug.Log(previousLaser);
        Debug.Log(previousGlow);
    }

    public override void Execute(BossStateMachine bossStateMachine)
    {
        if (!attacked) {
            if (bossEnemy.bossTwo && bossStateMachine.previousState == bossStateMachine.chase) {
                animator.SetBool("laser", true);
            } else {
                animator.SetBool("ranged", true);
            }
            attackAnimationStart = Time.time;
            animationTime = Time.time + attackTimer;
            attacked = true;

            if (bossEnemy.bossTwo) {
                if (bossEnemy.inStageOne) {
                    bossEnemy.RangedAttack(1, attackTimer);
                } else if (bossEnemy.inStageTwo) {
                    if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                        bossEnemy.RangedAttack(1, attackTimer);
                    } else if (bossStateMachine.previousState == bossStateMachine.chase) {
                        bossEnemy.RangedAttack(3, attackTimer);
                    } else if (bossStateMachine.previousState == bossStateMachine.rangedAttack) {
                        bossEnemy.RangedAttack(2, attackTimer);
                    }
                }
            } else if (bossEnemy.bossThree) {
                bossEnemy.RangedAttack(0, attackTimer);
            }
        }

        if (Time.time >= animationTime) {
            if (bossEnemy.bossTwo && bossStateMachine.previousState == bossStateMachine.chase && bossEnemy.inStageTwo) {
                animator.SetBool("laser", false);
                animator.SetBool("idle", false);
            } else {
                animator.SetBool("ranged", false);
            }
        }

        if (bossEnemy.bossTwo && bossEnemy.inStageOne) {
            if (bossStateMachine.previousState == bossStateMachine.rangedAttack) {
                bossStateMachine.nextState = bossStateMachine.chase;
            } else if (bossStateMachine.previousState == bossStateMachine.idle) {
                if (endLoopCounter == 3) {
                    bossStateMachine.nextState = bossStateMachine.idle;
                    bossStateMachine.endLoop = true;
                } else {
                    bossStateMachine.nextState = bossStateMachine.rangedAttack;
                }
            }
        } else if (bossEnemy.bossTwo && bossEnemy.inStageTwo && !nextStateSet) {
            if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                bossStateMachine.nextState = bossStateMachine.meleeAttack;
            } else if (bossStateMachine.previousState == bossStateMachine.chase) {
                bossStateMachine.nextState = bossStateMachine.rangedAttack;
                previousLaser = true;
                previousGlow = false;
            } else if (bossStateMachine.previousState == bossStateMachine.rangedAttack && previousLaser) {
                Debug.Log("Hello");
                bossStateMachine.nextState = bossStateMachine.rangedAttack;
                previousLaser = false;
                previousGlow = true;
            } else if (bossStateMachine.previousState == bossStateMachine.rangedAttack && previousGlow) {
                Debug.Log("Fuck");
                bossStateMachine.nextState = bossStateMachine.idle;
            }
            nextStateSet = true;
        } else if (bossEnemy.bossThree) {
            if (bossEnemy.inStageOne) {
                if (bossStateMachine.previousState == bossStateMachine.rangedAttack) {
                    bossStateMachine.nextState = bossStateMachine.idle;
                } else if (bossStateMachine.previousState == bossStateMachine.chase) {
                    bossStateMachine.nextState =bossStateMachine.chase;
                } else if (bossStateMachine.previousState == bossStateMachine.idle) {
                    bossStateMachine.nextState = bossStateMachine.rangedAttack;
                }
            }
        }

        if (Time.time - attackAnimationStart > bossEnemy.idleRangedTime) {
            if (bossEnemy.bossTwo && bossStateMachine.previousState == bossStateMachine.chase && bossEnemy.inStageTwo) {
                animator.SetBool("idle", true);
            }
            Exit(bossStateMachine);
        }
    }

    public override void Exit(BossStateMachine bossStateMachine)
    {
        if (bossStateMachine.endLoop) {
            endLoopCounter = 0;
        }
        bossStateMachine.TransitionState(bossStateMachine.nextState);
    }
}
