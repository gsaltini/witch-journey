using System.Collections.Generic;
using UnityEngine;

public class BossJumpState : BossState
{
    bool isJumping, jumpTimeSet, firstJump;
    int jumpCounter;
    float groundedTime;
    Transform playerTransform, groundPoint;
    Rigidbody2D rigidbody2D;
    BossEnemy bossEnemy;
    Vector2 groundSize, jumpForce;
    LayerMask groundLayer;
    Animator animator;

    public override void Enter(BossStateMachine bossStateMachine)
    {
        playerTransform = bossStateMachine.playerTransform;
        rigidbody2D = bossStateMachine.rigidBody;
        bossEnemy = bossStateMachine.bossEnemy;
        jumpForce = bossEnemy.jumpForce;
        groundPoint = bossEnemy.groundPoint;
        groundSize = bossEnemy.groundPointSize;
        groundLayer = bossEnemy.groundLayer;
        animator = bossStateMachine.animator;
        isJumping = false;
        jumpCounter = 0;

        animator.SetBool("idle", true);
        Debug.Log("Entering Jumping Away");
    }

    public override void Execute(BossStateMachine bossStateMachine)
    {
        if (Physics2D.OverlapBox(groundPoint.position, groundSize, 0, groundLayer)) {
            animator.SetBool("jump", false);
            isJumping = false;

            if(!isJumping && !jumpTimeSet) {
                groundedTime = Time.time;
                jumpTimeSet = true;
            }
            if (Mathf.Abs(playerTransform.position.x - bossStateMachine.transform.position.x) <= bossEnemy.meleeAttackRange) {
                if (bossEnemy.bossOne) {
                    bossStateMachine.nextState = bossStateMachine.meleeDown;
                }
                Exit(bossStateMachine);
            } else if (Time.time - groundedTime > 1.0f || firstJump){
                rigidbody2D.velocity = new Vector2(0f,0f);
                float playerSide = bossStateMachine.transform.position.x - playerTransform.position.x;
                if (playerSide > 0) {
                    rigidbody2D.AddForce(new Vector2(-jumpForce.x, jumpForce.y), ForceMode2D.Impulse);
                } else {
                    rigidbody2D.AddForce(new Vector2(jumpForce.x, jumpForce.y), ForceMode2D.Impulse);
                }
                animator.SetBool("jump", true);
                jumpCounter++;
                isJumping = true;
                firstJump = false;
                jumpTimeSet = false;
            }
        } else {
            animator.SetFloat("velocity_y", rigidbody2D.velocity.y);
        }

    }

    public override void Exit(BossStateMachine bossStateMachine)
    {
        bossStateMachine.TransitionState(bossStateMachine.nextState);
    }
}
