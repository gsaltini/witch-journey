using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{   
    public Transform playerTransform;
    private Vector3 originalPosition;
    [HideInInspector] public Unit unit;
    public State currentState;
    [HideInInspector] public State moveRight, moveLeft, moveUp, moveDown, chase, death, idle, attack;
    [HideInInspector] public State nextState;
    public bool followingPath = false;
    [HideInInspector] public BoxCollider2D boxCollider2D;
    [HideInInspector] public CapsuleCollider2D capsuleCollider2D;
    [HideInInspector] public Health health;
    //public Animator animator;
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public Rigidbody2D rigidBody;
    [HideInInspector] public Animator animator;

    void Start()
    {
        unit = GetComponent<Unit>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        health = GetComponent<Health>();
        sprite = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        originalPosition = transform.position;

        moveRight = (MoveRightState)ScriptableObject.CreateInstance(typeof(MoveRightState));
        moveLeft = (MoveLeftState)ScriptableObject.CreateInstance(typeof(MoveLeftState));
        chase = (ChaseState)ScriptableObject.CreateInstance(typeof(ChaseState));
        death = (DeathState)ScriptableObject.CreateInstance(typeof(DeathState));
        idle = (IdleState)ScriptableObject.CreateInstance(typeof(IdleState));
        
        if (enemy.meleeAttack) {
            attack = (MeleeAttackState)ScriptableObject.CreateInstance(typeof(MeleeAttackState));
        } else {
            attack = (RangedAttackState)ScriptableObject.CreateInstance(typeof(RangedAttackState));
        }

        if (unit.flying && enemy.verticalMovement) {
            moveUp = (MoveUpState)ScriptableObject.CreateInstance(typeof(MoveUpState));
            moveDown = (MoveDownState)ScriptableObject.CreateInstance(typeof(MoveDownState));
        }

        if (enemy.constantAim || enemy.constantMelee) {
            currentState = attack;
        } else {
            currentState = idle;
        }
        

        currentState.Enter(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Execute(this);
    }

    public void TransitionState(State state) 
    {
        if (followingPath) {
            unit.StopPathPosition();
        }
        currentState = state;
        currentState.Enter(this);
    }

    public Vector3 GetOriginalPosition() {
        return originalPosition;
    }

    public void SetOriginalPosition(Vector3 _originalPosition) {
        originalPosition = _originalPosition;
    }
    // TODO: Add components needed by the grunt for its various states, such as initial position and whatnot
}


