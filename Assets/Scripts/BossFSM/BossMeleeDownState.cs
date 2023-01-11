using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeDownState : BossState
{
    Transform attackPoint;
    BossEnemy bossEnemy;
    Animator animator;
    float nextAttackTime = 0f, attackAnimation = 5.0f, attackAnimationStart, attackTimer, animationTime;
    bool attacked;

    public override void Enter(BossStateMachine bossStateMachine)
    {
        bossEnemy = bossStateMachine.bossEnemy;
        attackPoint = bossEnemy.meleePointTwo;
        attacked = false;
        animator = bossStateMachine.animator;
        animator.SetBool("idle", true);

        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips) {
            if (clip.name == "Attack 2") {
                attackTimer = clip.length;
            }
        }
        Debug.Log(attackTimer);
    }

    public override void Execute(BossStateMachine bossStateMachine)
    {
        if (!attacked) {
            animator.SetBool("attack2", true);
            attackAnimationStart = Time.time;
            animationTime = Time.time + attackTimer;
            attacked = true;
            bossEnemy.MeleeAttack(2);
        }

        if (bossStateMachine.transform.position.x - bossStateMachine.playerTransform.position.x > 0) {
            bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.back);
        } else {
            bossStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }

        if (Time.time >= animationTime) {
            animator.SetBool("attack2", false);
        }

        if ( bossStateMachine.previousState == bossStateMachine.jump) {
            bossStateMachine.nextState = bossStateMachine.meleeUp;
        } else if (bossStateMachine.previousState == bossStateMachine.meleeUp) {
            bossStateMachine.nextState = bossStateMachine.jumpAway;
        } else {
            bossStateMachine.nextState = bossStateMachine.jump;
        }

        if (Time.time - attackAnimationStart > 2.0f) {
            Exit(bossStateMachine);
        }

    }

    public override void Exit(BossStateMachine bossStateMachine)
    {
        animator.SetBool("idle", true);
        animator.SetBool("attack1", false);
        bossStateMachine.TransitionState(bossStateMachine.nextState);
    }
}
