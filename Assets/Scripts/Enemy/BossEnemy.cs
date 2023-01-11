using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{

    [Header("Boss Instance")]
    public bool bossOne;
    public bool bossTwo, bossThree;
    [Header("Boss Stage Settings")]
    public bool stageOne;
    public bool stageTwo, stageThree;
    public bool inStageOne, inStageTwo, inStageThree;
    [HideInInspector] public bool fightStarted = false;
    [Header("Boss General Settings")]
    public float speed;
    public int damageResistance;
    public int contactDamage;
    [Header("Boss Melee Settings")]
    public int meleeOneDamage;
    public int meleeTwoDamage;
    [HideInInspector] public float meleeAttackRange;
    public float attackOneRange, attackTwoRange;
    [Header("Boss Ranged Settings")]
    public float rangedAttackRange;
    public int rangedOneDamage, rangedTwoDamage, rangedThreeDamage;
    [Header("Boss Projectile Settings")]
    public float rangedAttackTravelTime;
    public float projectileOneSpeed, projectileTwoSpeed;
    [Header("Boss Idle Settings")]
    public float idleTime;
    public float idleStartUpTime, startUpTime, idleMeleeTime, idleRangedTime;

    [Header("Boss Layer Settings")]
    public LayerMask playerMask;
    public LayerMask groundLayer;
    [Header("Boss Transforms")]
    public Transform meleePointOne;
    public Transform meleePointTwo, rangedPoint, rangedPointTwo, groundPoint;
    [Header("Boss Vectors")]
    public Vector2 groundPointSize;
    public Vector2 jumpForce;
    [Header("Boss Projectiles")]
    public GameObject projectileOne;
    public GameObject projectileTwo, projectileThree;
    BossHealth bossHealth;

    // Start is called before the first frame update
    void Start()
    {
        bossHealth = GetComponent<BossHealth>();
        meleeAttackRange = Mathf.Abs(meleePointOne.position.x - transform.position.x);
        inStageOne = true;
        inStageTwo = false;
        inStageThree = false;
    }

    public void MeleeAttack(int attackNumber) {
        if (bossOne) {
            if (attackNumber == 1) {
                StartCoroutine(MeleeUp());
            } else if (attackNumber == 2) {
                StartCoroutine(MeleeDown());
            } else {
                Debug.Log("Attack doesn't exist");
            }
        } else if (bossTwo) {
            StartCoroutine(MeleeGolem());
        } else if (bossThree) {
            if (attackNumber == 1) {
                StartCoroutine(BloatedArmMelee());
            } else if (attackNumber == 2) {
                StartCoroutine(BloatedGroundMelee());
            }
        }

    }

    public void RangedAttack(int attackNumber, float timer) {
        if (bossTwo) {
            if (attackNumber == 1) {
                StartCoroutine(RangedGolem());
            } else if (attackNumber == 2) {
                StartCoroutine(RangedGlowingGolem());
            } else if (attackNumber == 3) {
                StartCoroutine(GolemLaser(timer));
            }
        } else if (bossThree) {
            StartCoroutine(BloatedRanged());
        }

        //TODO: Instantiate projectile and set variables
    }

    IEnumerator MeleeUp() {
        yield return new WaitForSeconds(0.25f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(meleePointOne.position, attackOneRange, playerMask);

        if (colliders.Length > 0) {
            PlayerMovement player = colliders[0].GetComponent<PlayerMovement>();
            if (player != null) {

                player.takeDamage(meleeOneDamage);

                KnockBack(player);

                Debug.Log("Attack Up inflicted " + meleeOneDamage + " to " + colliders[0].name + "!");
            }
        } else {
            Debug.Log("Attack up missed");
        }
    }

    IEnumerator MeleeDown() {
        yield return new WaitForSeconds(0.4167f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(meleePointTwo.position, attackTwoRange, playerMask);

        if (colliders.Length > 0) {
            PlayerMovement player = colliders[0].GetComponent<PlayerMovement>();
            if (player != null) {
                player.takeDamage(meleeOneDamage);

                KnockBack(player);

                Debug.Log("Attack Down inflicted " + meleeOneDamage + " to " + colliders[0].name + "!");
            }  
        } else {
            Debug.Log("Attack down missed");
        }
    }

    IEnumerator MeleeGolem() {
        yield return new WaitForSeconds(1.16667f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(meleePointOne.position, attackOneRange, playerMask);

        if (colliders.Length > 0) {
            PlayerMovement player = colliders[0].GetComponent<PlayerMovement>();
            if (player != null) {
                player.takeDamage(meleeOneDamage);
                KnockBack(player);
                Debug.Log("Golem melee Attack inflicted " + meleeOneDamage + " to " + colliders[0].name + "!");
            }
        } else {
            Debug.Log("Golem melee missed");
        }
    }

    IEnumerator BloatedGroundMelee() {
        yield return new WaitForSeconds(0.33338f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(meleePointTwo.position, attackTwoRange, playerMask);

        if (colliders.Length > 0) {
            PlayerMovement player = colliders[0].GetComponent<PlayerMovement>();
            if (player != null) {
                player.takeDamage(meleeTwoDamage);
                KnockBack(player);
                Debug.Log("Golem melee Attack inflicted " + meleeTwoDamage + " to " + colliders[0].name + "!");
            }
        } else {
            Debug.Log("Golem melee missed");
        }
    }

    IEnumerator BloatedArmMelee() {
        yield return new WaitForSeconds(0.6667f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(meleePointOne.position, attackOneRange, playerMask);

        if (colliders.Length > 0) {
            PlayerMovement player = colliders[0].GetComponent<PlayerMovement>();
            if (player != null) {
                player.takeDamage(meleeOneDamage);
                KnockBack(player);
                Debug.Log("Golem melee Attack inflicted " + meleeOneDamage + " to " + colliders[0].name + "!");
            }
        } else {
            Debug.Log("Golem melee missed");
        }

    }

    IEnumerator RangedGolem() {
        yield return new WaitForSeconds(1.0f);

        GameObject golemProjectile = Instantiate(projectileOne, rangedPoint.position, rangedPoint.rotation);

        Bullet projectileScript = golemProjectile.GetComponent<Bullet>();
        if (transform.position.x - golemProjectile.transform.position.x > 0) {
            projectileScript.rightDirection = false;
        } else {
            projectileScript.rightDirection = true;
        }
        projectileScript.bulletDamage = rangedOneDamage;
        projectileScript.bulletLifeSpan = rangedAttackTravelTime;
        projectileScript.bulletSpeed = projectileOneSpeed;
    }

    IEnumerator RangedGlowingGolem() {
        yield return new WaitForSeconds(1.0f);

        GameObject golemProjectile = Instantiate(projectileTwo, rangedPoint.position, rangedPoint.rotation);

        Bullet projectileScript = golemProjectile.GetComponent<Bullet>();
        if (transform.position.x - golemProjectile.transform.position.x > 0) {
            projectileScript.rightDirection = false;
        } else {
            projectileScript.rightDirection = true;
        }
        projectileScript.bulletDamage = rangedTwoDamage;
        projectileScript.bulletLifeSpan = rangedAttackTravelTime;
        projectileScript.bulletSpeed = projectileTwoSpeed;
    }

    IEnumerator GolemLaser(float timer) {
        yield return new WaitForSeconds(timer);

        GameObject golemLaser = Instantiate(projectileThree, rangedPointTwo.position, rangedPoint.rotation);

        Laser projectileScript = golemLaser.GetComponent<Laser>();
        projectileScript.laserDamage = rangedThreeDamage;
    }

    IEnumerator BloatedRanged() {
        yield return new WaitForSeconds(0.5f);

        GameObject blob = Instantiate(projectileOne, rangedPoint.position, rangedPoint.rotation);

        Bullet projectileScript = blob.GetComponent<Bullet>();
        if (transform.position.x - blob.transform.position.x > 0) {
            projectileScript.rightDirection = false;
        } else {
            projectileScript.rightDirection = true;
        }
        projectileScript.bulletDamage = rangedOneDamage;
        projectileScript.bulletLifeSpan = rangedAttackTravelTime;
        projectileScript.bulletSpeed = projectileOneSpeed;
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

            player.takeDamage(contactDamage);

            KnockBack(player);
        }
    }

    void OnDrawGizmos() {
        if (meleePointOne != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(meleePointOne.position, attackOneRange);
        }
        if (meleePointTwo != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(meleePointTwo.position, attackTwoRange);
        }
        if (rangedPoint != null) {
            Gizmos.DrawWireSphere(rangedPoint.position, 0.5f);
        }

        if (rangedPointTwo != null) {
            Gizmos.DrawWireSphere(rangedPointTwo.position, 0.5f);
        }

        if (groundPoint != null) {
            Gizmos.DrawCube(groundPoint.position, groundPointSize);
        }
        
    }

}
