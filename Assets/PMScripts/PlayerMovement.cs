using System;
using UnityEngine;

/// <summary>
/// Player movement class.
/// Followed the following tutorials found here: 
/// https://www.youtube.com/watch?v=TcranVQUQ5U
/// https://www.youtube.com/watch?v=KCzEnKLaaPc
/// 
/// To use this class, the character must have a RigidBody2D component, a BoxCollider2D component,
/// as well as a transform added to check for walls for wall jumps.
/// and other items the player should detect (like the ground) should have BoxCollider2D components.
/// 
/// Walls should have a "wall" layer added and ground should have a "ground" layer added to them.
/// 
/// The serialized parameters below can be tuned as needed for different characters.
/// 
/// Items to be picked up using this script need to have a 2D collider attached to them,
/// As well as an ItemPickup script. They need to be layered with the "pickup" layer.
/// 
/// Health has been normalized to be "full" at 1000, but we can change that as needed.
/// 
/// Recommended values to start out with:
/// 
/// Speed: 10
/// Jump Power: 5
/// Flight Power: 1
/// X and Y Wall Force: 3
/// Wall Jump Time: 0.05
/// Check Radius: 3
/// Pickup Radius: 3
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float jumpPower;
    [SerializeField] public float flightPower;
    [SerializeField] public bool canDoubleJump = true;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] public float xWallForce;
    [SerializeField] public float yWallForce;
    [SerializeField] public float wallJumpTime;
    [SerializeField] private float checkRadius;
    [SerializeField] private float pickupRadius;
    [SerializeField] private int attacksRemaining = 99999;
    [SerializeField] private int health = 1000;
    private DateTime doubleJumpTimer;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private float distToGround;
    private bool isTouchingFront;
    private bool wallJumping;

    public int attackSelected;
    public Transform frontCheck;
    //Animator anim;
    public float wallSlidingSpeed;
    public bool flightEnabled = true;
    public Animator animator;

    [HideInInspector] public float knockBackCount;
    [Header("Knock Back Settings")]
    public float knockBackLength;
    [HideInInspector] public bool knockBackRight, knockBackSet;
    public Vector2 knockBackStrength;

    // Need float variable for animator movement use
    float horizontalMove = 0f;

    /// <summary>
    /// Init components
    /// </summary>
    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        //anim = GetComponent<Animator>();
        animator = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("ground");
        wallLayer = LayerMask.GetMask("wall");
        pickupLayer = LayerMask.GetMask("pickup");

        knockBackSet = false;
    }

    /// <summary>
    /// this is running regularly and is the main function for player movement capabilities
    /// </summary>
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        horizontalMove = horizontalInput * speed;

        // Adds Animator Parameter that allows player to transition from idle state to run state, and vice versa
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Added a knockBackTimer that starts counting down once the player collides with the trigger collider set around the
        // enemies.
        if (knockBackCount <= 0) {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            knockBackSet = false;
        } else {
            if (knockBackRight && !knockBackSet) {
                body.velocity = new Vector2(knockBackStrength.x, knockBackStrength.y);
                knockBackSet = true;
            } else if (!knockBackRight && !knockBackSet) {
                body.velocity = new Vector2(-knockBackStrength.x, knockBackStrength.y);
                knockBackSet = true;
            }
            knockBackCount -= Time.deltaTime;
        }
        


        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);


        if (Physics2D.OverlapCircle(frontCheck.position, checkRadius, groundLayer))
        {
            isTouchingFront = true;
        }

        //wall jumping
        if (Input.GetKeyDown(KeyCode.UpArrow) && isTouchingFront && horizontalInput != 0)
        {
            wallJumping = true;
            Invoke("setWallJumpFalse", wallJumpTime);
        }

        if (wallJumping)
        {
            body.velocity = new Vector2(xWallForce * -horizontalInput, yWallForce);
        }

        //jumping and double jumping
        if (Input.GetKey(KeyCode.Space))
        {

            distToGround = boxCollider.bounds.extents.y;
            if (Physics2D.Raycast(transform.position, Vector2.down, distToGround + .1f, groundLayer))
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                doubleJumpTimer = DateTime.Now;
                canDoubleJump = true;
            }
            else if ((DateTime.Now - doubleJumpTimer).Milliseconds > 300 && canDoubleJump)
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                canDoubleJump = false;
            } 
        }

        //flight
        if (Input.GetKey(KeyCode.F) && flightEnabled)
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }

        // Attacking 
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("IsAttacking", true);
        }

        // picking up items
        // press "p" to pickup items in front of the character within checkRadius
        if (Input.GetKeyDown(KeyCode.P))
        {
            var itemToPickup = Physics2D.OverlapCircle(frontCheck.position, pickupRadius, pickupLayer);
            if (itemToPickup != null)
            {
                var obj = itemToPickup.gameObject;
                // this should always happen here (i.e. items with pickup layer should have ItemPickup script)
                // but we can check just in case
                var itemPickupScript = obj.GetComponent<ItemPickup>();
                if (itemPickupScript != null)
                {
                    itemPickupScript.PickUp();
                }
            }
        }
    }

    /// <summary>
    /// helper function that disables wall jumping
    /// </summary>
    private void setWallJumpFalse()
    {
        wallJumping = false;
    }

    /// <summary>
    /// Character can attack if attacks are remaining
    /// </summary>
    /// <returns></returns>
    public bool canAttack()
    {
        return attacksRemaining > 0;
    }

    /// <summary>
    /// update the attack type
    /// 
    /// 0: fire
    /// 1: water
    /// 2: earth
    /// 3: air
    /// </summary>
    /// <param name="value"></param>
    public void updateAttackSelected(int value)
    {
        if (value > -1 && value < 4)
        {
            attackSelected = value;
        }
    }

    /// <summary>
    /// Function to call to take damage
    /// Damage argument has default value of 1
    /// </summary>
    /// <param name="_damage"></param>
    public void takeDamage(int _damage = 1)
    {
        health -= _damage;
        Debug.Log("Player took " + _damage + " damage! Current health is " + health + ".");

        GameObject.FindObjectOfType<PowerBar>().TakeDamage();

        if (health <= 0) {
            Debug.Log("Player has died!");
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    /// <summary>
    /// Function to call to increase health
    /// </summary>
    /// <param name="_damage"></param>
    public void addHealth(int _health)
    {
        health += _health;
    }

    /// <summary>
    /// Function to call to get player's health value
    /// </summary>
    /// <param name="_damage"></param>
    public int getHealth()
    {
        return health;
    }
}
