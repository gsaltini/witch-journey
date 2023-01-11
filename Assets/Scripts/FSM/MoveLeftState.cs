using UnityEngine;
using UnityEngine.AI;
using System;

[CreateAssetMenu(menuName = "FSM/State/MoveLeft")]
public class MoveLeftState : State
{
    public Transform enemyTransform;
    public Transform playerTransform;
    Animator animator;
    public float movementSpeed = 0.8f;

    public override void Enter(StateMachine stateMachine) {
        enemyTransform = stateMachine.transform;
        playerTransform = stateMachine.playerTransform;
        animator = stateMachine.animator;

        enemyTransform.rotation = Quaternion.LookRotation(Vector3.back);
    }

    public override void Execute(StateMachine stateMachine) {
        Vector3 newDestination = stateMachine.GetOriginalPosition() + Vector3.left * stateMachine.enemy.distanceMovedLeft;
        float horizontalDistance = Math.Abs(enemyTransform.position.x - playerTransform.position.x);
        float verticalDistance = Math.Abs(enemyTransform.position.y - playerTransform.position.y);

        if (horizontalDistance < 3.0f && verticalDistance < 3.0f && stateMachine.enemy.aggressive) {
            stateMachine.nextState = stateMachine.chase;
            stateMachine.unit.StopPathPosition();
            stateMachine.followingPath = false;
            Exit(stateMachine);
        } else if (enemyTransform.position.x >= newDestination.x + 1.0f) {
            if (!stateMachine.followingPath) {
                stateMachine.followingPath = true;
                stateMachine.unit.StartPath(newDestination);
                animator.SetBool("moving", true);
            }
        } else {
            stateMachine.nextState = stateMachine.idle;
            stateMachine.unit.StopPathPosition();
            stateMachine.followingPath = false;
            Exit(stateMachine);
        }
    }

    public override void Exit(StateMachine stateMachine) {
        animator.SetBool("moving", false);
        stateMachine.TransitionState(stateMachine.nextState);

    }
}
