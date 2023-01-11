using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10.0f;
    Rigidbody2D rigidBody2D;
    PlayerMovement playerMovement;
    public int bulletDamage = 0;
    float time;
    public float bulletLifeSpan;
    public bool rightDirection;


    // Start is called before the first frame update
    void Start()
    {   
        rigidBody2D = GetComponent<Rigidbody2D>();
        if (rightDirection) {
            rigidBody2D.velocity = transform.right * bulletSpeed;
        } else {
            rigidBody2D.velocity = transform.right * bulletSpeed;
        }
        
        time = Time.time;
    }

    void Update() {
        if (Time.time - time > bulletLifeSpan) {
            Debug.Log("Bullet expired!");
            Destroy(gameObject);
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

            playerMovement.takeDamage(bulletDamage);
        }
        Debug.Log("Dealt " + bulletDamage + " damage to " + collider.name);
        Destroy(gameObject);
    }
}
