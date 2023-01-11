using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BossState
{
    Transform playerTransform, enemyTransform;
    Rigidbody2D rigidbody2D;
    BossEnemy bossEnemy;
    Animator animator;
    float movementSpeed;
    bool velocitySet;
    int loopCounter = 0;
    public override void Enter(BossStateMachine bossStateMachine)
    {
        playerTransform = bossStateMachine.playerTransform;
        enemyTransform = bossStateMachine.transform;
        rigidbody2D = bossStateMachine.rigidBody;
        bossEnemy = bossStateMachine.bossEnemy;
        movementSpeed = bossEnemy.speed;
        velocitySet = false;
        animator = bossStateMachine.animator;

        if (bossEnemy.bossOne) {
            if (bossStateMachine.previousState == bossStateMachine.idle || bossStateMachine.previousState == bossStateMachine.rangedAttack) {
                if (bossStateMachine.transform.position.x - bossStateMachine.playerTransform.position.x > 0) {
                    bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.back);
                } else {
                    bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                }
            } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                if (bossStateMachine.transform.position.x - bossStateMachine.originalPosition.x > 0) {
                    bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.back);
                } else {
                    bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                }
            }
        } else if (bossEnemy.bossThree) {
            if (bossStateMachine.previousState == bossStateMachine.idle || bossStateMachine.previousState == bossStateMachine.rangedAttack) {
                if (bossStateMachine.transform.position.x - bossStateMachine.playerTransform.position.x < 0) {
                    bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.back);
                } else {
                    bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                }
            } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                if (bossStateMachine.transform.position.x - bossStateMachine.originalPosition.x < 0) {
                    bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.back);
                } else {
                    bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                }
            }
        }

        Debug.Log("Entering chase state");
    }

    public override void Execute(BossStateMachine bossStateMachine)
    {

        if (bossStateMachine.previousState == bossStateMachine.idle || bossStateMachine.previousState == bossStateMachine.rangedAttack) {
            float playerSide = playerTransform.position.x - enemyTransform.position.x;
            if (playerSide > 0) {
                if (bossEnemy.bossOne || bossEnemy.bossThree)
                    animator.SetBool("moving", true);
                else if (bossEnemy.bossTwo)
                    animator.SetBool("idle", true);
                rigidbody2D.velocity = new Vector2(1.0f * movementSpeed, rigidbody2D.velocity.y);
                velocitySet = true;
            } else if (playerSide < 0) {
                if (bossEnemy.bossOne || bossEnemy.bossThree)
                    animator.SetBool("moving", true);
                else if (bossEnemy.bossTwo)
                    animator.SetBool("idle", true);
                rigidbody2D.velocity = new Vector2(1.0f * -movementSpeed, rigidbody2D.velocity.y);
                velocitySet = true;
            }
            if (Mathf.Abs(playerSide) < bossEnemy.meleeAttackRange) {
                if (bossEnemy.bossOne) {
                    bossStateMachine.nextState = bossStateMachine.meleeUp;
                } else if (bossEnemy.bossTwo && bossEnemy.inStageOne || bossEnemy.inStageTwo) {
                    bossStateMachine.nextState = bossStateMachine.meleeAttack;
                } else if (bossEnemy.bossThree) {
                    bossStateMachine.nextState = bossStateMachine.meleeAttack;
                }
                rigidbody2D.velocity = Vector2.zero;
                Exit(bossStateMachine);
            }
        } else if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
            float originalSide = bossStateMachine.originalPosition.x - enemyTransform.position.x;
            if (originalSide > 0) {
                if (bossEnemy.bossTwo) {
                    animator.SetBool("idle", true);
                } else if (bossEnemy.bossThree) {
                    animator.SetBool("moving", true);
                }
                rigidbody2D.velocity = new Vector2(1.0f * movementSpeed, rigidbody2D.velocity.y);
                velocitySet = true;
            } else if (originalSide < 0) {
                if (bossEnemy.bossTwo) {
                    animator.SetBool("idle", true);
                } else if (bossEnemy.bossThree) {
                    animator.SetBool("moving", true);
                }
                rigidbody2D.velocity = new Vector2(-1.0f * movementSpeed, rigidbody2D.velocity.y);
            }
            if (Mathf.Abs(originalSide) < 0.5f) {
                if (bossEnemy.bossTwo && bossEnemy.inStageOne) {
                    if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                        bossStateMachine.nextState = bossStateMachine.idle;
                    } else if (bossStateMachine.previousState == bossStateMachine.rangedAttack) {
                        bossStateMachine.nextState = bossStateMachine.idle;
                    }
                } else if (bossEnemy.bossTwo && bossEnemy.inStageTwo) {
                    if (bossStateMachine.previousState == bossStateMachine.meleeAttack) {
                        bossStateMachine.nextState = bossStateMachine.rangedAttack;
                    }
                } if (bossEnemy.bossThree) {
                    if (bossEnemy.inStageOne) {
                        if (loopCounter == 0) {
                            bossStateMachine.nextState = bossStateMachine.rangedAttack;
                            loopCounter++;
                        } else if (loopCounter == 1) {
                            bossStateMachine.nextState = bossStateMachine.idle;
                            loopCounter++;
                        }
                    }
                }

                rigidbody2D.velocity = Vector2.zero;
                Exit(bossStateMachine);
            }
        }
    }

    public override void Exit(BossStateMachine bossStateMachine)
    {
        if (bossEnemy.bossOne || bossEnemy.bossThree) {
            animator.SetBool("moving", false);
            if (bossEnemy.bossThree && loopCounter > 1) {
                loopCounter = 0;
            }
        }   
        else if (bossEnemy.bossTwo) {
            animator.SetBool("idle", false);
        }  
        bossStateMachine.TransitionState(bossStateMachine.nextState);
    }
}

