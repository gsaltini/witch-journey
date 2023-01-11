using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    StateMachine stateMachine;
    SpriteRenderer spriteRenderer;
    Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateMachine = GetComponent<StateMachine>();

        currentHealth = maxHealth;
        defaultColor = spriteRenderer.color;
    }

    public void TakeDamage(float amount) {
        currentHealth = currentHealth - amount;
        StartCoroutine(HitColor());
        
        if (currentHealth <= 0) {
            Death();
        }
    }

    IEnumerator HitColor() {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = defaultColor;
    }

    private void Death() {
        stateMachine.TransitionState(stateMachine.death);
    }
}
