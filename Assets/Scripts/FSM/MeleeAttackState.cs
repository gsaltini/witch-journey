using System.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : State
{
    Transform enemyTransform, playerTransform, attackPoint;
    float attackRange, attackCenter, attackTimer, animationTime;
    float nextAttackTime = 0f;
    Animator animator;
    
    public override void Enter(StateMachine stateMachine)
    {
        enemyTransform = stateMachine.transform;
        playerTransform = stateMachine.playerTransform;
        attackPoint = stateMachine.enemy.attackPoint;
        attackRange = stateMachine.enemy.meleeRange;
        attackCenter = enemyTransform.position.x + attackPoint.position.x;
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, stateMachine.enemy.playerLayer);
        if (!stateMachine.enemy.constantMelee && colliders.Length > 0) {
            if (Time.time > nextAttackTime) {
                stateMachine.enemy.MeleeAttack(colliders[0]);
                animationTime = Time.time + attackTimer;
                nextAttackTime = Time.time + 1 / stateMachine.enemy.meleeAttackRate;
            }

            if (Time.time > animationTime) {
                animator.SetBool("attack", false);  
            }

        } else if (stateMachine.enemy.constantMelee) {
            stateMachine.enemy.MeleeAttack(null);
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
        stateMachine.TransitionState(stateMachine.nextState);
    }   
}
