using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BossState
{
    Animator animator;
    BoxCollider2D boxCollider2D;
    CapsuleCollider2D capsuleCollider2D;
    Rigidbody2D rigidbody2D;
    float enterTime, deathTimer;
    public override void Enter(BossStateMachine bossStateMachine)
    {
        animator = bossStateMachine.animator;
        boxCollider2D = bossStateMachine.boxCollider2D;
        rigidbody2D = bossStateMachine.rigidBody;
        capsuleCollider2D = bossStateMachine.capsuleCollider2D;

        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips) {
            if (clip.name == "Death") {
                deathTimer = clip.length;
            }
        }

        enterTime = Time.time;

        Debug.Log("Boss man Died");
    }

    public override void Execute(BossStateMachine bossStateMachine)
    {
        animator.SetBool("dead", true);
        boxCollider2D.enabled = false;
        capsuleCollider2D.enabled = false;
        rigidbody2D.isKinematic = true;
        rigidbody2D.Sleep();

        if (Time.time - enterTime > deathTimer) {
            Destroy(bossStateMachine.gameObject);
        }
    }

    public override void Exit(BossStateMachine bossStateMachine)
    {
        throw new System.NotImplementedException();
    }
}
