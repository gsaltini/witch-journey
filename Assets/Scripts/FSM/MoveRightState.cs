using UnityEngine;
using UnityEngine.AI;
using System;

[CreateAssetMenu(menuName = "FSM/State/MoveRight")]
public class MoveRightState : State
{
    Transform enemyTransform;
    Transform playerTransform;
    Animator animator;

    float movementSpeed = 1.2f;

    public override void Enter(StateMachine stateMachine) {
        enemyTransform = stateMachine.transform;
        playerTransform = stateMachine.playerTransform;
        animator = stateMachine.animator;

        enemyTransform.rotation = Quaternion.LookRotation(Vector3.forward);
        
    }

    public override void Execute(StateMachine stateMachine) {
        Vector3 newDestination = stateMachine.GetOriginalPosition() + Vector3.right * stateMachine.enemy.distanceMovedRight;
        float horizontalDistance = Math.Abs(enemyTransform.position.x - playerTransform.position.x);
        float verticalDistance = Math.Abs(enemyTransform.position.y - playerTransform.position.y);

        if (horizontalDistance < 3.0f && verticalDistance < 5.0f && stateMachine.enemy.aggressive) {
            Debug.Log(horizontalDistance);
            Debug.Log(verticalDistance);
            stateMachine.nextState = stateMachine.chase;
            stateMachine.unit.StopPathPosition();
            stateMachine.followingPath = false;
            Exit(stateMachine);
        } else if (enemyTransform.position.x <= newDestination.x - 1.0f) {
            if (!stateMachine.followingPath) {
                stateMachine.followingPath = true;
                stateMachine.unit.StartPath(newDestination);
                animator.SetBool("moving", true);
            }
        } else {
            stateMachine.followingPath = false;
            stateMachine.unit.StopPathPosition();
            stateMachine.nextState = stateMachine.idle;
            Exit(stateMachine);
        }
    }

    public override void Exit(StateMachine stateMachine) {
        animator.SetBool("moving", false);
        stateMachine.TransitionState(stateMachine.nextState);
    }
}
