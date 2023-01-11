using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 1000;
    float currentHealth;
    public int stageTwoCutOff = 500;
    public int stageThreeCutOff = 200;
    BossStateMachine bossStateMachine;
    BossEnemy bossEnemy;
    SpriteRenderer spriteRenderer;
    Color defaultColor;
    // Start is called before the first frame update
    void Start()
    {   
        bossEnemy = GetComponent<BossEnemy>();
        bossStateMachine = GetComponent<BossStateMachine>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        defaultColor = spriteRenderer.color;
    }

    public void TakeDamage(float damageAmount) {
        currentHealth = currentHealth - damageAmount;
        StartCoroutine(HitColor());
        Debug.Log(currentHealth);

        if (bossEnemy.stageTwo && currentHealth < stageTwoCutOff && !bossEnemy.inStageTwo && !bossEnemy.inStageThree) {
            bossEnemy.speed = bossEnemy.speed * 1.5f;
            bossEnemy.damageResistance = bossEnemy.damageResistance * 2;
            bossStateMachine.TransitionState(bossStateMachine.stageTransition);
            bossEnemy.inStageOne = false;
            bossEnemy.inStageTwo = true;
        }
        if (bossEnemy.stageThree && currentHealth < stageThreeCutOff && !bossEnemy.stageThree) {
            bossStateMachine.TransitionState(bossStateMachine.stageTransition);
            bossEnemy.inStageThree = true;
            bossEnemy.inStageTwo = false;
            bossEnemy.inStageOne = false;
        }
        if (currentHealth <= 0) {
            Death();
        }
    }

    IEnumerator HitColor() {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = defaultColor;
    }

    void Death() {
        bossStateMachine.TransitionState(bossStateMachine.death);
    }
}
