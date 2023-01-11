using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Tutorial source: https://www.youtube.com/watch?v=PUpC44Q64zY&list=PLgOEwFbvGm5o8hayFB6skAfa8Z-mw4dPV&index=4
/// 
/// This object requires a 2D box collider and at least for now, an animator component as well.
/// It requires a transform to check if an object is in front of it.
/// 
/// It terminates upon reaching an object with layer canhit.
/// 
/// When you make this object, you must assign it to the player movement script of the character you want to use it.
/// 
/// Suggestion: set speed to 3 and checkRadius to 0.5 to start out with.
/// </summary>
public class EarthBlock : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float checkRadius = 0.5f;
    [SerializeField] private LayerMask canHitLayer;
    private float direction;
    private float lifetime;


    private BoxCollider2D boxCollider;
    private Animator anim;
    public Transform frontCheck;

    private void Awake()
    {
        transform.localScale = new Vector3(1, 1, 1);
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        canHitLayer = LayerMask.GetMask("canhit");
    }

    private void Update()
    {
        bool hit = Physics2D.OverlapCircle(frontCheck.position, checkRadius, canHitLayer);

        if (hit)
        {
            gameObject.SetActive(false);
        }

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.position = new Vector3(transform.position.x + movementSpeed, transform.position.y, transform.position.z);


        lifetime += Time.deltaTime;
        if (lifetime > 5)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (_direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

    }

    public void IncreaseDamage()
    {
        damage = damage + 10;
    }
}
