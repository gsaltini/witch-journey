using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Tutorial source: https://www.youtube.com/watch?v=PUpC44Q64zY&list=PLgOEwFbvGm5o8hayFB6skAfa8Z-mw4dPV&index=4
/// 
/// Assign this script to the character you want to have attacking ability
/// 
/// Right now, attacks when player left clicks
/// 
/// Suggestions: ensure attack cooldown is more than 0 to prevent firing all attack projectiles at once
/// (i.e. 0.5 or something)
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float fireAttackCooldown;
    [SerializeField] private float waterAttackCooldown;
    [SerializeField] private float airAttackCooldown;
    [SerializeField] private float earthAttackCooldown;
    [SerializeField] private float currentAttackCooldown;

    //[SerializeField] private Dictionary<string, float> attackCooldowns;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private GameObject[] airBursts;
    [SerializeField] private GameObject[] waterBalls;
    [SerializeField] private GameObject[] earthBlocks;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        /*
        attackCooldowns.Add("fire", 0.7f);
        attackCooldowns.Add("water", 0.7f);
        attackCooldowns.Add("earth", 0.7f);
        attackCooldowns.Add("air", 0.7f);
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            playerMovement.updateAttackSelected(0);
        }
        if (Input.GetKeyDown("1"))
        {
            playerMovement.updateAttackSelected(1);
        }
        if (Input.GetKeyDown("2"))
        {
            playerMovement.updateAttackSelected(2);
        }
        if (Input.GetKeyDown("3"))
        {
            playerMovement.updateAttackSelected(3);
        }

        if (Input.GetMouseButton(0) && cooldownTimer > currentAttackCooldown && playerMovement.canAttack())
        {
            // Should always be 0-3
            switch (playerMovement.attackSelected)
            {
                case 0:
                    FireAttack();
                    break;
                case 1:
                    WaterAttack();
                    break;
                case 2:
                    EarthAttack();
                    break;
                case 3:
                    AirAttack();
                    break;
                default:
                    break;
            }
        }

        cooldownTimer += Time.deltaTime;
    }

    // Fire
    private void FireAttack()
    {
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Fireball>().SetDirection(Mathf.Sign(transform.localScale.x));

        currentAttackCooldown = fireAttackCooldown;
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    // Water
    private void WaterAttack()
    {
        cooldownTimer = 0;

        waterBalls[FindWaterball()].transform.position = firePoint.position;
        waterBalls[FindWaterball()].GetComponent<Waterball>().SetDirection(Mathf.Sign(transform.localScale.x));

        currentAttackCooldown = waterAttackCooldown;

    }

    private int FindWaterball()
    {
        for (int i = 0; i < waterBalls.Length; i++)
        {
            if (!waterBalls[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    // Earth
    private void EarthAttack()
    {
        cooldownTimer = 0;

        earthBlocks[FindEarthBlock()].transform.position = firePoint.position;
        earthBlocks[FindEarthBlock()].GetComponent<EarthBlock>().SetDirection(Mathf.Sign(transform.localScale.x));

        currentAttackCooldown = earthAttackCooldown;

    }

    private int FindEarthBlock()
    {
        for (int i = 0; i < earthBlocks.Length; i++)
        {
            if (!earthBlocks[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    // Air
    private void AirAttack()
    {
        cooldownTimer = 0;

        airBursts[FindAirBurst()].transform.position = firePoint.position;
        airBursts[FindAirBurst()].GetComponent<Airburst>().SetDirection(Mathf.Sign(transform.localScale.x));

        currentAttackCooldown = airAttackCooldown;

    }

    private int FindAirBurst()
    {
        for (int i = 0; i < earthBlocks.Length; i++)
        {
            if (!earthBlocks[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    public void LevelUpAttack(string s)
    {
        if (s == "fire")
        {
            for (int i = 0; i < fireballs.Length; i++)
            {
                fireballs[i].GetComponent<Fireball>().IncreaseDamage();
            }
        }
        
        else if (s == "water")
        {
            for (int i = 0; i < waterBalls.Length; i++)
            {
                waterBalls[i].GetComponent<Waterball>().IncreaseDamage();
            }
        }
        else if (s == "air")
        {
            for (int i = 0; i < fireballs.Length; i++)
            {
                airBursts[i].GetComponent<Airburst>().IncreaseDamage();
            }
        }
        else if (s == "earth")
        {
            for (int i = 0; i < fireballs.Length; i++)
            {
                earthBlocks[i].GetComponent<EarthBlock>().IncreaseDamage();
            }
        }
        

    }

    public void LevelUpCooldown(string s)
    {
        if (s == "fire")
        {
            fireAttackCooldown -= 0.1f;
        }
        else if (s == "water")
        {
            waterAttackCooldown -= 0.1f;
        }
        else if (s == "air")
        {
            airAttackCooldown -= 0.1f;
        }
        else if (s == "earth")
        {
            earthAttackCooldown -= 0.1f;
        }
    }
}
