using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "FSM/State/Idle")]
public class IdleState : State
{
    private float time;
    private bool startUp = true;
    private Transform playerTransform, enemyTransform;
    private Animator animator;
    public override void Enter(StateMachine stateMachine)
    {
        time = Time.time;
        playerTransform = stateMachine.playerTransform;
        enemyTransform = stateMachine.transform;
        animator = stateMachine.animator;
        animator.SetBool("idle", true);
    }

    public override void Execute(StateMachine stateMachine)
    {
        // if (startUp) {
        //     stateMachine.SetOriginalPosition(enemyTransform.position);
        //     startUp = false;
        // }

        float horizontalDistance = Math.Abs(enemyTransform.position.x - playerTransform.position.x);
        float verticalDistance = Math.Abs(enemyTransform.position.y - playerTransform.position.y);

        if (horizontalDistance < stateMachine.enemy.aggroRange && verticalDistance < stateMachine.enemy.aggroRange && stateMachine.enemy.mobile && stateMachine.enemy.aggressive) {
            Debug.Log(verticalDistance);
            stateMachine.nextState = stateMachine.chase;
            Exit(stateMachine);
        }

        if (Time.time - time > stateMachine.enemy.idleTime) {
            if (stateMachine.enemy.mobile) {
                if (stateMachine.enemy.verticalMovement && stateMachine.unit.flying) {
                    if (enemyTransform.position.y > stateMachine.GetOriginalPosition().y) {
                        stateMachine.nextState = stateMachine.moveDown;
                    } else {
                        stateMachine.nextState = stateMachine.moveUp;
                    }
                    Exit(stateMachine);
                } else {
                    if (enemyTransform.position.x > stateMachine.GetOriginalPosition().x) {
                        stateMachine.nextState = stateMachine.moveLeft;
                        
                    } else {
                        stateMachine.nextState = stateMachine.moveRight;
                    }
                    Exit(stateMachine);
                }
            } else if (stateMachine.enemy.aggressive) {
                stateMachine.nextState = stateMachine.attack;
                Exit(stateMachine);
            } else if (stateMachine.enemy.constantAim) {
                stateMachine.nextState = stateMachine.attack;
                Exit(stateMachine);
            } else if (stateMachine.enemy.constantMelee) {
                stateMachine.nextState = stateMachine.attack;
                Exit(stateMachine);
            }
            
        }

        if (Vector3.Distance(enemyTransform.position, playerTransform.position) <= 1.5f && stateMachine.enemy.aggressive) {
            stateMachine.nextState = stateMachine.attack;
            Exit(stateMachine);
        }
    }

    public override void Exit(StateMachine stateMachine)
    {
        animator.SetBool("idle", false);
        stateMachine.TransitionState(stateMachine.nextState);
    }
}
