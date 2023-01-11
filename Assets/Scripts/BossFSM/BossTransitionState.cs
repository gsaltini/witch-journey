using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTransitionState : BossState
{
    float armorTimer, animationTime, animationStartTimer;
    bool armored;
    Animator animator;
    BossEnemy bossEnemy;
    public override void Enter(BossStateMachine bossStateMachine)
    {
        animator = bossStateMachine.animator;
        bossEnemy = bossStateMachine.bossEnemy;
        armored = false;
        
        if (bossEnemy.bossTwo) {
            AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in animationClips) {
                if (clip.name == "Armor Buff") {
                    armorTimer = clip.length;
                }
            }
        }

        Debug.Log(armorTimer);
    }

    public override void Execute(BossStateMachine bossStateMachine)
    {
        if (bossEnemy.bossTwo) {
            if (!armored) {
                animator.SetBool("idle", false);
                animator.SetBool("melee", false);
                animator.SetBool("ranged", false);
                animator.SetBool("armor", true);

                bossEnemy.StopAllCoroutines();

                animationStartTimer = Time.time;
                animationTime = Time.time + armorTimer;
                armored = true;
            }


            if (Time.time > animationTime) {
                animator.SetBool("armor", false);
                bossStateMachine.nextState = bossStateMachine.idle;
                Exit(bossStateMachine);
            }
        }


    }

    public override void Exit(BossStateMachine bossStateMachine)
    {
        bossStateMachine.TransitionState(bossStateMachine.nextState);
    }
}
