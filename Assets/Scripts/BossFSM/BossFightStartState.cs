using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightStartState : BossState
{
    Animator animator;
    BossEnemy bossEnemy;
    float time, animationTimer;
    bool animating;
    public override void Enter(BossStateMachine bossStateMachine)
    {
        //animator = bossStateMachine.animator;
        time = Time.time;
        bossEnemy = bossStateMachine.bossEnemy;
        animator = bossStateMachine.animator;

        
        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips) {
            if (clip.name == "Glowing" && bossEnemy.bossTwo) {
                animationTimer = clip.length;
            } else if (clip.name == "Sneer" && bossEnemy.bossThree) {
                animationTimer = clip.length;
            }
        }

        animating = false;
        Debug.Log("Starting boss fight");
    }

    public override void Execute(BossStateMachine bossStateMachine)
    {
        //animator.startFight;
        if (bossEnemy.bossOne) {
            if (Time.time - time > bossEnemy.startUpTime) {
                bossStateMachine.nextState = bossStateMachine.idle;
                Exit(bossStateMachine);
            }
        } else if (bossEnemy.bossTwo) {
            if (!animating) {
                animator.SetBool("glowing", true);
                time = Time.time + animationTimer;
                animating = true;
            }

            if (Time.time >= time) {
                animator.SetBool("glowing", false);
                bossStateMachine.nextState = bossStateMachine.idle;
                Exit(bossStateMachine);
            }
        } else if (bossEnemy.bossThree) {
            if (!animating) {
                animator.SetBool("sneer", true);
                time = Time.time + animationTimer;
                animating = true;
            }

            if (Time.time >= time) {
                animator.SetBool("sneer", false);
                bossStateMachine.nextState = bossStateMachine.idle;
                Exit(bossStateMachine);
            }
        }
    }

    public override void Exit(BossStateMachine bossStateMachine)
    {
        bossEnemy.fightStarted = true;
        bossStateMachine.TransitionState(bossStateMachine.nextState);
    }
}
