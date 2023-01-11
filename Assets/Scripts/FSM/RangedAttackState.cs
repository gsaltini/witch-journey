using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : State
{
    Transform enemyTransform, playerTransform, firePointTransform;
    float nextAttackTime = 0f;
    Vector3 attackDirection;
    float angle, attackTimer, animationStart, animationTime;
    Animator animator;

    public override void Enter(StateMachine stateMachine)
    {
        enemyTransform = stateMachine.transform;
        playerTransform = stateMachine.playerTransform;
        firePointTransform = stateMachine.enemy.firePoint;

        if (!stateMachine.enemy.constantAim && stateMachine.unit.flying) {
            attackDirection = playerTransform.position - firePointTransform.position;
            angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            firePointTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        animator = stateMachine.animator;
        animator.SetBool("idle", true);

        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips) {
            if (clip.name == "Attack") {
                attackTimer = clip.length;
            }
        }
        
    }

    public override void Execute(StateMachine stateMachine)
    {
        if (Vector3.Distance(playerTransform.position, enemyTransform.position) < stateMachine.enemy.rangedAttackRange && !stateMachine.enemy.constantAim) {
            if (Time.time > nextAttackTime) {
                animationTime = Time.time + attackTimer;
                animator.SetBool("attack", true);
                animationStart = Time.time;
                if (enemyTransform.position.x - playerTransform.position.x > 0) {
                    enemyTransform.rotation = Quaternion.LookRotation(Vector3.back);
                } else {
                    enemyTransform.rotation = Quaternion.LookRotation(Vector3.forward);
                }
                
                if (stateMachine.unit.flying) {
                    attackDirection = playerTransform.position - firePointTransform.position;
                    angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
                    firePointTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }


                stateMachine.enemy.RangedAttack(); 
                nextAttackTime = Time.time + 1 / stateMachine.enemy.rangedAttackRate;
            }

            if (Time.time > animationTime) {
                animator.SetBool("attack", false);  
            }
        } else if (stateMachine.enemy.constantAim) {
            stateMachine.enemy.RangedAttack(); 
            nextAttackTime = Time.time + 1 / stateMachine.enemy.rangedAttackRate;
            stateMachine.nextState = stateMachine.idle;
            Exit(stateMachine);
        } else {
            stateMachine.nextState = stateMachine.idle;
            Exit(stateMachine);
        }
    }

    public override void Exit(StateMachine stateMachine)
    {
        animator.SetBool("attack", false);
        animator.SetBool("idle", false);
        stateMachine.enemy.firePoint.rotation = enemyTransform.rotation;
        stateMachine.TransitionState(stateMachine.nextState);
    }
}
