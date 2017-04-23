﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum AnimState
    {
        IDLE,
        WALKING,
        SWORD_SLASH,
        BOW_FIRE,
        MAGIC_ATTACK,
    }

    public AnimState animState = AnimState.IDLE;
    public float attackAnimTimer = 0.0f;
    public float attackAnimDuration = 0.0f;
    public float animTimer = 0.0f;
    public float animScale = 1.0f;
    public float animSpeed = 1.0f;

    public bool isPlayer = false;
    public Attack[] attacks;
    public int currentAttack;
    public float maxHealth = 10.0f;
    public float health = 10.0f;
    public float healthRegenRate = 0.0f;
    public float moveSpeed = 1.0f;
    public int XP = 0;

    public int facing = 0;
    public SpriteRenderer body;
    public Transform leftHand, rightHand;
    public Sprite[] directionalSprites = new Sprite[4];

    public DungeonPiece currentRoom;

    public DamageNumbers damageNumbersPrefab;

    public float maxInvulnerabilityCooldown = 1.0f;
    public float invulnerabilityCooldown = 1.0f;

    public Rigidbody2D rb;

    public Vector2 crosshair;

	protected float attackArcMultiplier = 1.0F;
	protected float meleeDamageMultiplier = 1.0F;

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
        UpdateAnimations();

        health = Mathf.Min(health + healthRegenRate * Time.deltaTime, maxHealth);
    }

    public virtual void FixedUpdate() { }

    public void SetAttackAnimState(AnimState s, float f) { animState = s; attackAnimDuration = f; attackAnimTimer = 0.0f; }
    public void SetAnimState(AnimState s) { if (attackAnimTimer > attackAnimDuration) animState = s; }

    protected void UpdateAnimations()
    {
        Vector3 move = rb.velocity;
        if (move.x < move.y) // UL
        {
            if (move.x + move.y < 0) // L
            {
                SetFacing(1);
            }
            else // U
            {
                SetFacing(0);
            }
        }
        else // RD
        {
            if (move.x + move.y < 0) // D
            {
                SetFacing(2);
            }
            else // R
            {
                SetFacing(3);
            }
        }

        animTimer += Time.deltaTime * animSpeed;

        body.transform.localPosition = new Vector3(0.0f, Mathf.Sin(-2.0f * animTimer), 0.0f) * 0.01f * animScale;
        attackAnimTimer += Time.deltaTime;

        float fParametric = attackAnimTimer / attackAnimDuration;

        rightHand.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        leftHand.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        switch (animState)
        {
                case AnimState.IDLE:
                {
                    leftHand.localPosition = (new Vector3(-0.25f, -0.2f, -0.1f) + new Vector3(0.0f, Mathf.Sin(2.0f * animTimer), 0.0f) * 0.02f) * animScale;
                    rightHand.localPosition = (new Vector3(0.25f, -0.2f, -0.1f) + new Vector3(0.0f, Mathf.Sin(2.0f * animTimer), 0.0f) * 0.02f) * animScale;
                    break;
                }

                case AnimState.WALKING:
                {
                    float fSin = Mathf.Sin(8.0f * animTimer);
                    float fCos = Mathf.Cos(8.0f * animTimer + Mathf.PI / 2.0f) * 0.125f;
                    switch (facing)
                    {
                            case 0: // U
                            {
                                rightHand.localPosition = (new Vector3(0.25f, -0.2f, 0.1f) + new Vector3(fCos, fSin, 0.0f) * 0.12f) * animScale;
                                leftHand.localPosition = (new Vector3(-0.25f, -0.2f, 0.1f) + new Vector3(fCos, -fSin, 0.0f) * 0.12f) * animScale;
                                break;
                            }
                            case 1: // L
                            {
                                leftHand.localPosition = (new Vector3(0.25f, -0.2f, -0.1f) + new Vector3(fSin, fCos, 0.0f) * 0.12f) * animScale;
                                rightHand.localPosition = (new Vector3(-0.25f, -0.2f, 0.1f) + new Vector3(-fSin, fCos, 0.0f) * 0.12f) * animScale;
                                break;
                            }
                            case 2: // D
                            {
                                leftHand.localPosition = (new Vector3(0.25f, -0.2f, -0.1f) + new Vector3(fCos, -fSin, 0.0f) * 0.12f) * animScale;
                                rightHand.localPosition = (new Vector3(-0.25f, -0.2f, -0.1f) + new Vector3(fCos, fSin, 0.0f) * 0.12f) * animScale;
                                break;
                            }
                            case 3: // R
                            {
                                leftHand.localPosition = (new Vector3(0.25f, -0.2f, 0.1f) + new Vector3(-fSin, fCos, 0.0f) * 0.12f) * animScale;
                                rightHand.localPosition = (new Vector3(-0.25f, -0.2f, -0.1f) + new Vector3(fSin, fCos, 0.0f) * 0.12f) * animScale;
                                break;
                            }
                    }
                    break;
                }
                case AnimState.SWORD_SLASH: // SWORD
                {
                    float fInitialAngle = -facing * 90.0f - 90.0f;
                    float fFinalAngle = -facing * 90.0f + 90.0f;
                    //-2x ^ 3 + 3x ^ 2
                    fParametric = fParametric * fParametric * (3 - 2 * fParametric);

                    float fCurrentAngle = fInitialAngle + (fFinalAngle - fInitialAngle) * fParametric;

                    bool bBehind = Mathf.Abs(Mathf.DeltaAngle(fCurrentAngle, 0.0f)) <= 90.0f;
                        

                    rightHand.localPosition = new Vector3(Mathf.Sin(Mathf.Deg2Rad * fCurrentAngle) * 0.4f, -0.1f + 0.75f * Mathf.Cos(Mathf.Deg2Rad * fCurrentAngle) * 0.4f, bBehind ? 0.1f : -0.1f) * animScale;
                    rightHand.localEulerAngles = new Vector3(0.0f, 0.0f, -fCurrentAngle + 90.0f);


                    float fOffHand = fCurrentAngle + 180.0f;

                    leftHand.localPosition = new Vector3(Mathf.Sin(Mathf.Deg2Rad * fOffHand) * 0.4f, -0.1f + 0.75f * Mathf.Cos(Mathf.Deg2Rad * fOffHand) * 0.4f, bBehind ? -0.1f : 0.1f) * animScale;
                    leftHand.localEulerAngles = new Vector3(0.0f, 0.0f, -fOffHand + 90.0f);

                    break;
                }
        }
        
    }

    protected void SetFacing(int dir)
    {
        if (attackAnimTimer > attackAnimDuration)
        {
            facing = dir;
            body.sprite = directionalSprites[dir];
        }
    }

    protected void UseCurrentAttack(int attackMode)
    {
        if (attacks != null && currentAttack >= 0 && currentAttack < attacks.Length)
        {
            attacks[currentAttack].TryToUse(attackMode, transform.position, (Vector3)crosshair - transform.position);
        }
    }

    public void Damage(Attack attack)
    {
        if(attack.parent.isPlayer == isPlayer)
        {
            return;
        }
        if(invulnerabilityCooldown <= 0.0f)
        {
            // Do damage;
			float fDamage = Random.Range(attack.minDamage, attack.maxDamage) * attack.parent.getMeleeDamageMultiplier();

            invulnerabilityCooldown = maxInvulnerabilityCooldown;
            health -= fDamage;
            // Add some numbers
            DamageNumbers numbers = Instantiate<DamageNumbers>(damageNumbersPrefab);
            numbers.transform.position = transform.position + new Vector3(0.0f, 0.5f * animScale, 0.0f);
            numbers.SetNumber(Mathf.RoundToInt(fDamage));

            if (health < 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        if(this != Game.thePlayer)
        {
            Game.thePlayer.XP += XP;
        }
        Destroy(gameObject);
    }

	public virtual float getAttackArcMultiplier()
	{
		return attackArcMultiplier;
	}
	public virtual float getMeleeDamageMultiplier()
	{
		return meleeDamageMultiplier;
	}
}
