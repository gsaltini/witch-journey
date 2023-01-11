using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Type Settings")]
    public bool meleeAttack;
    public bool mobile, verticalMovement, aggressive, constantAim, constantMelee;
    [Header("Range Settings")]
    public float aggroRange = 3.0f;
    public float aggroMeleeRange = 0.5f;
    public float meleeRange = 0.5f;
    public float rangedAttackRange = 10.0f;
    [Header("Projectile Settings")]
    public int rangedAttackSpeed = 5;
    public float bulletTravelTime = 5.0f;
    public GameObject bulletPrefab;
    [Header("Damage Settings")]
    public int meleeDamage = 20;
    public int rangedDamage = 15;
    public int contactDamage = 5;
    [Header("Attack Rate Settings")]
    public float meleeAttackRate = 0.5f;
    public float rangedAttackRate = 0.3f;
    [Header("Idle Setting")]
    public float idleTime = 5.0f;
    [Header("Movement Settings")]
    public float distanceMovedRight = 5.0f;
    public float distanceMovedLeft = 5.0f;
    public float distanceMovedUp = 5.0f;
    public float distanceMovedDown = 5.0f;
    StateMachine stateMachine;
    [Header("Attack Origin Settings")]
    public Transform attackPoint;
    public Transform firePoint;
    [Header("Player Settings")]
    public LayerMask playerLayer;
    Health health;
    Animator animator;
    private float animatorTimer;
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();

        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in animationClips) {
            if (clip.name == "Attack") {
                animatorTimer = clip.length;
            }
        }
        Debug.Log(animatorTimer);
    }

    public void MeleeAttack(Collider2D collider) {
        StartCoroutine(Melee(collider));
    }

    IEnumerator Melee(Collider2D collider) {
        animator.SetBool("attack", true);

        if (gameObject.name == "NightBorne") {
            yield return new WaitForSeconds(animatorTimer - (animatorTimer/12)*2);
        } else {
            yield return new WaitForSeconds(animatorTimer / 2);
        }


        if (collider != null) {
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();

            if (playerMovement != null) {
                playerMovement.takeDamage(meleeDamage);

                KnockBack(playerMovement);
            }
            Debug.Log("The AI unit hit " + collider.name + " for " + meleeDamage + " damage!");
        } else {
            Debug.Log("Constant Attack");
        }
    }

    public void RangedAttack() {
        StartCoroutine(Ranged());
    }

    IEnumerator Ranged() {
        //animator.SetBool("attack", true);

        yield return new WaitForSeconds(animatorTimer / 2);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (transform.position.x - bullet.transform.position.x > 0) {
            bulletScript.rightDirection = false;
        } else {
            bulletScript.rightDirection = true;
        }
        bulletScript.bulletDamage = rangedDamage;
        bulletScript.bulletLifeSpan = bulletTravelTime;
        bulletScript.bulletSpeed = rangedAttackSpeed;
        
    }

    void KnockBack(PlayerMovement player) {
        if (player.transform.position.x - transform.position.x > 0) {
            player.knockBackRight = true;
        } else {
            player.knockBackRight = false;
        }

        player.knockBackCount = player.knockBackLength;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "Red Witch") {
            PlayerMovement player = collider.GetComponent<PlayerMovement>();

            KnockBack(player);

            player.takeDamage(contactDamage);
        }
    }

    void OnDrawGizmos() {
        if (attackPoint != null) {
            Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
        }
        if (firePoint != null) {
            Gizmos.DrawWireSphere(firePoint.position, meleeRange);
        }
        Gizmos.DrawWireSphere(transform.position, aggroMeleeRange);
        
    }

}
