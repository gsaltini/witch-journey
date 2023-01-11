using System.Security.Claims;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/Death")]
public class DeathState : State
{
    Animator animator;
    float deathTimer, enterTime;
    public override void Enter(StateMachine stateMachine)
    {
        Debug.Log("Died");
        animator = stateMachine.animator;
        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips) {
            if (clip.name == "Death") {
                deathTimer = clip.length;
            }
        }
        enterTime = Time.time;
    }
    public override void Execute(StateMachine stateMachine)
    {
        // Destroy(stateMachine.gameObject);
        animator.SetBool("dead", true);
        stateMachine.boxCollider2D.enabled = false;
        stateMachine.capsuleCollider2D.enabled = false;
        stateMachine.rigidBody.isKinematic = true;
        stateMachine.rigidBody.Sleep();

        if (Time.time - enterTime > deathTimer) {
            Destroy(stateMachine.gameObject);
        }

        
        
    }
    public override void Exit(StateMachine stateMachine)
    {
        throw new NotImplementedException();
    }
}
