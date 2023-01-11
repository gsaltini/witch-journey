using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveDownState : State
{
    Transform playerTransform, enemyTransform;
    public override void Enter(StateMachine stateMachine)
    {
        enemyTransform = stateMachine.transform;
        playerTransform = stateMachine.playerTransform;
    }

    public override void Execute(StateMachine stateMachine)
    {
        Vector3 newDestination = stateMachine.GetOriginalPosition() + Vector3.down * stateMachine.enemy.distanceMovedDown;
        float horizontalDistance = Math.Abs(enemyTransform.position.x - playerTransform.position.x);
        float verticalDistance = Math.Abs(enemyTransform.position.y - playerTransform.position.y);

        if (horizontalDistance < 3.0f && stateMachine.enemy.aggressive) {
            stateMachine.nextState = stateMachine.chase;
            stateMachine.unit.StopPathPosition();
            stateMachine.followingPath = false;
            Exit(stateMachine);
        } else if (enemyTransform.position.y >= newDestination.y + 1.0f) {
            if (!stateMachine.followingPath) {
                stateMachine.followingPath = true;
                stateMachine.unit.StartPath(newDestination);
            }
        } else {
            stateMachine.followingPath = false;
            stateMachine.unit.StopPathPosition();
            stateMachine.nextState = stateMachine.idle;
            Exit(stateMachine);
        }
    }

    public override void Exit(StateMachine stateMachine)
    {
        stateMachine.TransitionState(stateMachine.nextState);
    }
}
