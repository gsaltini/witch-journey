using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    PlayerMovement playerMovement;
    Animator animator;
    public int laserDamage = 0;
    float time, laserTimer, laserLifeSpan;


    // Start is called before the first frame update
    void Start()
    {   
        time = Time.time;

        animator = GetComponent<Animator>();

        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in animationClips) {
                if (clip.name == "InAir") {
                    laserLifeSpan = clip.length;
                }
            }
    }

    void Update() {
        if (laserLifeSpan <= 0) {
            Destroy(gameObject);
        } else {
            laserLifeSpan -= Time.deltaTime;
        }
    }   

    void KnockBack(PlayerMovement player) {
        if (player.transform.position.x - transform.position.x > 0) {
            player.knockBackRight = true;
        } else {
            player.knockBackRight = false;
        }

        player.knockBackCount = player.knockBackLength;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        playerMovement = collider.GetComponent<PlayerMovement>();

        if (playerMovement != null) {
            KnockBack(playerMovement);

            playerMovement.takeDamage(laserDamage);
        }
    }
}
