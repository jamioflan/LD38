using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Attack[] attacks;
    public int currentAttack;
    public float maxHealth = 10.0f;
    public float health = 10.0f;
    public float moveSpeed = 1.0f;

    public int facing = 0;
    public SpriteRenderer body, leftHand, rightHand;
    public Sprite[] directionalSprites = new Sprite[4];

    public DungeonPiece currentRoom;

    public float maxInvulnerabilityCooldown = 1.0f;
    public float invulnerabilityCooldown = 1.0f;

    public Rigidbody2D rb;

    public Vector2 crosshair;

    public virtual void Awake()
    {

    }

    // Use this for initialization
    public virtual void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    public virtual void Update ()
    {
        invulnerabilityCooldown -= Time.deltaTime;

    }

    public virtual void FixedUpdate() { }

    protected void SetFacing(int dir)
    {
        facing = dir;
        body.sprite = directionalSprites[dir];
    }

    protected void UseCurrentAttack(int attackMode)
    {
        if (attacks != null && currentAttack >= 0 && currentAttack < attacks.Length)
        {
            attacks[currentAttack].Use(attackMode, transform.position, (Vector3)crosshair - transform.position);
        }
    }

    public void Damage(Attack attack)
    {
        if(invulnerabilityCooldown <= 0.0f)
        {
            // Do damage;
            invulnerabilityCooldown = maxInvulnerabilityCooldown;
            health -= attack.damage;
        }
       
    }
}
