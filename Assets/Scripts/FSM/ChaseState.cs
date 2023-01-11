using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/Chase")]
public class ChaseState : State
{

    Transform enemyTransform;
    Transform playerTransform;
    Transform attackPoint;
    public float movementSpeed = 1.0f;
    float attackRange;
    Animator animator;
    
    public override void Enter(StateMachine stateMachine) 
    {
        enemyTransform = stateMachine.transform;
        playerTransform = stateMachine.playerTransform;

        if (enemyTransform.position.x - playerTransform.position.x > 0) {
            enemyTransform.rotation = Quaternion.LookRotation(Vector3.back);
        } else {
            enemyTransform.rotation = Quaternion.LookRotation(Vector3.forward);
        }

        if (stateMachine.enemy.meleeAttack) {
            attackRange = stateMachine.enemy.meleeRange;
            attackPoint = stateMachine.enemy.attackPoint;
        } else {
            attackRange = stateMachine.enemy.rangedAttackRange;
            attackPoint = stateMachine.enemy.firePoint;
        }

        animator = stateMachine.animator;
    }

    public override void Execute(StateMachine stateMachine)
    {
        float side = enemyTransform.position.x - playerTransform.position.x;
        float height = enemyTransform.position.y - playerTransform.position.y;
        //Vector3.Distance(playerTransform.position, enemyTransform.position)

        if (MathF.Abs(side) > stateMachine.enemy.aggroRange) {
            stateMachine.followingPath = false;
            stateMachine.unit.StopPathPosition();
            stateMachine.nextState = stateMachine.idle;
            Exit(stateMachine);
        } else if (Mathf.Abs(attackPoint.transform.position.x - playerTransform.position.x)  <= attackRange && stateMachine.enemy.meleeAttack) {
            //Debug.Log(Mathf.Abs(stateMachine.enemy.attackPoint.transform.position.x - playerTransform.position.x) <= attackRange);
            stateMachine.followingPath = false;
            stateMachine.unit.StopPathPosition();
            stateMachine.nextState = stateMachine.attack;
            Exit(stateMachine);
        } else if (Mathf.Abs(attackPoint.transform.position.x - playerTransform.position.x)  <= attackRange) {
            stateMachine.followingPath = false;
            stateMachine.unit.StopPathPosition();
            stateMachine.nextState = stateMachine.attack;
            Exit(stateMachine);
        } else {
            if (!stateMachine.followingPath) {
                stateMachine.followingPath = true;
                if (Mathf.Abs(playerTransform.position.y - enemyTransform.position.y) > 0) {
                    Vector3 newPosition = new Vector3(playerTransform.position.x, enemyTransform.position.y, playerTransform.position.z);
                    stateMachine.unit.StartPath(newPosition);
                    animator.SetBool("moving", true);
                } else {
                    stateMachine.unit.StartPath(playerTransform.position);
                    animator.SetBool("moving", true);
                }
                
            }
        }
    }

    public override void Exit(StateMachine stateMachine)
    {
        animator.SetBool("moving", false);
        stateMachine.TransitionState(stateMachine.nextState);
    }
}
