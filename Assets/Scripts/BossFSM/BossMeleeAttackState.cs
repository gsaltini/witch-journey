using System.Linq.Expressions;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttackState : BossState
{
    Transform attackPoint;
    BossEnemy bossEnemy;
    Animator animator;
    float nextAttackTime = 0f, attackAnimation = 5.0f, attackAnimationStart, attackTimer, animationTime;
    bool attacked, groundSpike = false, armSpike = false, nextStateSet, animationStopped;

    public override void Enter(BossStateMachine bossStateMachine)
    {
        bossEnemy = bossStateMachine.bossEnemy;
        attackPoint = bossEnemy.meleePointOne;
        attacked = false;
        animator = bossStateMachine.animator;
        animator.SetBool("idle", true);

        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips) {
            if (clip.name == "Melee" && bossEnemy.bossTwo) {
                attackTimer = clip.length;
            } else if (bossEnemy.bossThree) {
                if (bossStateMachine.previousState == bossStateMachine.chase && clip.name == "Attack 1") {
                    attackTimer = clip.length;
                } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                    if (armSpike && clip.name == "Attack 2") {
                        attackTimer = clip.length;
                    } else if (groundSpike && clip.name == "Attack 1") {
                        attackTimer = clip.length;
                    }
                }
            }
        }
        
        nextStateSet = false;
        animationStopped = false;
        
        Debug.Log("Entering attack state");
        Debug.Log(attackTimer);

    }

    public override void Execute(BossStateMachine bossStateMachine)
    {

        if (!attacked) {
            if (bossEnemy.bossTwo) {
                animator.SetBool("melee", true);
            }  else if (bossEnemy.bossThree) {
                if (bossEnemy.inStageOne) {
                    if (bossStateMachine.previousState == bossStateMachine.chase) {
                        animator.SetBool("arm", true);
                    } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                        if (armSpike) {
                            animator.SetBool("ground", true);
                        } else if (groundSpike) {
                            animator.SetBool("arm", true);
                        }
                    }
                }
            }

            attackAnimationStart = Time.time;
            animationTime = Time.time + attackTimer;
            attacked = true;

            if (bossEnemy.bossTwo) {
                bossEnemy.MeleeAttack(0);
            } else if (bossEnemy.bossThree) {
                if (bossEnemy.inStageOne) {
                    if (bossStateMachine.previousState == bossStateMachine.chase) {
                        bossEnemy.MeleeAttack(1);
                    } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                        if (armSpike) {
                            bossEnemy.MeleeAttack(2);
                        } else if (groundSpike) {
                            bossEnemy.MeleeAttack(1);
                        }
                    }
                }
            }
             //TODO: Change melee attack helper
        }

        if (Time.time >= animationTime && !animationStopped) {
            if (bossEnemy.bossTwo) {
                animator.SetBool("melee", false);
            } else if (bossEnemy.bossThree) {
                if (bossEnemy.inStageOne) {
                    if (bossStateMachine.previousState == bossStateMachine.chase) {
                        animator.SetBool("arm", false);
                        animationStopped = true;
                    } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                        if (armSpike) {
                            animator.SetBool("ground", false);
                            animationStopped = true;
                        } else if (groundSpike) {
                            animator.SetBool("arm", false);
                            animationStopped = true;
                        }
                    }
                }
            }
        }
        
        if (bossEnemy.bossTwo && bossEnemy.inStageOne) {
            if (bossStateMachine.previousState == bossStateMachine.chase) {
                bossStateMachine.nextState = bossStateMachine.meleeAttack;
            } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                bossStateMachine.nextState = bossStateMachine.chase;
            }
        } else if (bossEnemy.bossTwo && bossEnemy.inStageTwo) {
            if (bossStateMachine.previousState == bossStateMachine.chase) {
                bossStateMachine.nextState = bossStateMachine.rangedAttack;
            } else if (bossStateMachine.previousState == bossStateMachine.rangedAttack) {
                bossStateMachine.nextState = bossStateMachine.chase;
            }
        } else if (bossEnemy.bossThree) {
            if (bossEnemy.inStageOne && !nextStateSet && animationStopped) {
                if (bossStateMachine.previousState == bossStateMachine.chase) {
                    bossStateMachine.nextState = bossStateMachine.meleeAttack;
                    groundSpike = false;
                    armSpike = true;
                    nextStateSet = true;
                } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack && armSpike) {
                    bossStateMachine.nextState = bossStateMachine.meleeAttack;
                    groundSpike = true;
                    armSpike = false;
                    nextStateSet = true;
                } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack && groundSpike) {
                    bossStateMachine.nextState = bossStateMachine.chase;
                    nextStateSet = true;
                }
            }
        }


        if (Time.time - attackAnimationStart > bossEnemy.idleMeleeTime) {
            Exit(bossStateMachine);
        }

    }

    public override void Exit(BossStateMachine bossStateMachine)
    {
        animator.SetBool("idle", true);
        animator.SetBool("attack1", false);
        bossStateMachine.TransitionState(bossStateMachine.nextState);
    }
}
