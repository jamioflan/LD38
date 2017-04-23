using System.Collections;
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
        LUNGE,
    }

    protected AnimState animState = AnimState.IDLE;
    public float attackAnimTimer = 0.0f;
    public float attackAnimDuration = 0.0f;
    protected float animTimer = 0.0f;
    public float animScale = 1.0f;
    public float animSpeed = 1.0f;

    public bool isPlayer = false;
	public bool isBoss = false;
    protected float bleedtimer = 0;
    public Attack[] attacks;
    public int currentAttack;
    public float maxHealth = 10.0f;
    public float health = 10.0f;
    public float healthRegenRate = 0.0f;
	public float bleedRate = 2.0f;
    public float moveSpeed = 1.0f;
    public int XP = 0;
    protected float timeSinceLastBleed = 0F;
	public float knockback = 1F;

	public List<Upgrade> upgrades = new List<Upgrade>();

    public int facing = 0;
    protected float attackMoveTimer = 0.0f;
    protected Vector2 attackMoveVector = Vector2.zero;
	
    public SpriteRenderer body;
    public Transform leftHand, rightHand;
    public Sprite[] directionalSprites = new Sprite[4];

	public BloodSplatter splatterPrefab;

    public DungeonPiece currentRoom;

    public DamageNumbers damageNumbersPrefab;

    public float maxInvulnerabilityCooldown = 1.0f;
    public float invulnerabilityCooldown = 1.0f;

    protected Rigidbody2D rb;

    public Vector2 crosshair;

	public float attackArcMultiplier = 1.0F;
	public float magicDistanceMultiplier = 1.0F;
	public float magicTimeMultiplier = 1.0F;
	public float meleeDamageMultiplier = 1.0F;
	public float rangedDamageMultiplier = 1.0F;
	public float magicDamageMultiplier = 1.0F;

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

		if (bleedtimer > 0)
		{
			health = Mathf.Max(health - bleedRate * Time.deltaTime, 0);
			bleedtimer -= Time.deltaTime;
			timeSinceLastBleed += Time.deltaTime;
			if (timeSinceLastBleed > 1)
			{
				timeSinceLastBleed = 0;
				BloodSplatter splat = Instantiate<BloodSplatter> (splatterPrefab);
				splat.transform.position = transform.position;
				splat.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.Range(0F,360F));
			}
		}

        health = Mathf.Min(health + healthRegenRate * Time.deltaTime, maxHealth);
    }

    public virtual void FixedUpdate() { }

    public void SetAttackAnimState(AnimState s, float f) { animState = s; attackAnimDuration = f; attackAnimTimer = 0.0f; }
    public void SetAnimState(AnimState s) { if (attackAnimTimer > attackAnimDuration) animState = s; }

    protected void UpdateAnimations()
    {
        if (rb == null) return;

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

        if(rightHand == null || leftHand == null)
        {
            return;
        }
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
            case AnimState.MAGIC_ATTACK:
              case AnimState.BOW_FIRE:
                {
                    Vector3 dPos = (Vector3)crosshair - transform.position;
                    float fCurrentAngle = -Mathf.Rad2Deg * Mathf.Atan2(dPos.y, dPos.x) + 90.0f;
                    bool bBehind = Mathf.Abs(Mathf.DeltaAngle(fCurrentAngle, 0.0f)) <= 90.0f;

                    rightHand.localPosition = new Vector3(Mathf.Sin(Mathf.Deg2Rad * fCurrentAngle) * 0.4f, -0.1f + 0.75f * Mathf.Cos(Mathf.Deg2Rad * fCurrentAngle) * 0.4f, bBehind ? 0.1f : -0.1f) * animScale;
                    rightHand.localEulerAngles = new Vector3(0.0f, 0.0f, -fCurrentAngle + 90.0f);
                    leftHand.localPosition = new Vector3(Mathf.Sin(Mathf.Deg2Rad * fCurrentAngle) * 0.2f, -0.1f + 0.75f * Mathf.Cos(Mathf.Deg2Rad * fCurrentAngle) * 0.2f, bBehind ? 0.1f : -0.1f) * animScale;
                    leftHand.localEulerAngles = new Vector3(0.0f, 0.0f, -fCurrentAngle + 90.0f);
                    break;
                }
            case AnimState.LUNGE:
                {

                    Vector3 dPos = (Vector3)crosshair - transform.position;
                    float fTargetAngle = -Mathf.Rad2Deg * Mathf.Atan2(dPos.y, dPos.x) + 90.0f;
                    float fInitialAngle = fTargetAngle - 30.0f;
                    float fFinalAngle = fTargetAngle + 30.0f;
                    float fCurrentAngle = fInitialAngle + (fFinalAngle - fInitialAngle) * fParametric;

                    bool bBehind = Mathf.Abs(Mathf.DeltaAngle(fCurrentAngle, 0.0f)) <= 90.0f;

                    rightHand.localPosition = new Vector3(0.1f, 0.0f, 0.0f);
                    rightHand.Rotate(0.0f, 0.0f, -fCurrentAngle + 90.0f);
                    //rightHand.localEulerAngles = new Vector3(0.0f, 0.0f, -fCurrentAngle + 90.0f);
                    rightHand.localPosition += new Vector3(Mathf.Sin(Mathf.Deg2Rad * fCurrentAngle) * 0.4f, -0.1f + 0.75f * Mathf.Cos(Mathf.Deg2Rad * fCurrentAngle) * 0.4f, bBehind ? 0.1f : -0.1f) * animScale;

                    leftHand.localPosition = new Vector3(-0.1f, 0.0f, 0.0f);
                    leftHand.Rotate(0.0f, 0.0f, -fCurrentAngle + 90.0f);
                    //leftHand.localEulerAngles = new Vector3(0.0f, 0.0f, -fCurrentAngle + 90.0f);
                    leftHand.localPosition += new Vector3(Mathf.Sin(Mathf.Deg2Rad * fCurrentAngle) * 0.4f, -0.1f + 0.75f * Mathf.Cos(Mathf.Deg2Rad * fCurrentAngle) * 0.4f, bBehind ? 0.1f : -0.1f) * animScale;

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

    public void SetAttackMoveVector(Vector2 moveDir, float duration)
    {
        attackMoveTimer = duration;
        attackMoveVector = moveDir;
    }

    public virtual void Damage(Attack attack)
    {
        if(attack.parent.isPlayer == isPlayer)
        {
            return;
        }
        if(invulnerabilityCooldown <= 0.0f)
        {
            // Do damage;
			float fDamage = Random.Range(attack.minDamage, attack.maxDamage) * attack.getDamageMultiplier();

            invulnerabilityCooldown = maxInvulnerabilityCooldown;
            health -= fDamage;

			// If the attacker has vampirism, heal them part of the damage
			if ((attack.attackType == Attack.AttackType.MELEE || attack.attackType == Attack.AttackType.MAGIC) && attack.parent.hasUpgrade("meleeMagicAttacksHealYou"))
			{
				attack.parent.health = Mathf.Min(attack.parent.health + fDamage * 0.2F, attack.parent.maxHealth);
			}

			// Knockback
			Vector3 knock = new Vector2(attack.parent.transform.position.x - transform.position.x,attack.parent.transform.position.y - transform.position.y);
			knock = knock.normalized * knockback;
			rb.MovePosition(knock);
            if (splatterPrefab != null)
            {
                BloodSplatter splat = Instantiate<BloodSplatter>(splatterPrefab);
                splat.transform.position = transform.position;
                splat.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.Range(0F, 360F));
            }
			// Set off the bleeding condition if necessary
			if (!isBoss && attack.parent.isPlayer && (attack.attackType == Attack.AttackType.MELEE || attack.attackType == Attack.AttackType.RANGED) && attack.parent.hasUpgrade("meleeRangedMultiattack"))
			{
				bleedtimer = 5F;
			}

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

    public virtual void Die()
    {
        if(this != Game.thePlayer)
        {
            Game.thePlayer.XP += XP;
        }
        Destroy(gameObject);
    }

	public virtual float getAttackArcMultiplier()
	{
		float multiplier = attackArcMultiplier;
		if (hasUpgrade("meleeIncreasedArc"))
		{
			multiplier = multiplier * 2F;
			if (hasUpgrade ("melee360"))
			{
				multiplier = multiplier * 2F;
			}
		}
		return multiplier;
	}
	public virtual float getMagicDistanceMultiplier()
	{
		float multiplier = magicDistanceMultiplier;
		if (hasUpgrade("magicIncreasedDistance"))
		{
			multiplier = multiplier * 1.5F;
		}
		return multiplier;
	}
	public virtual float getMagicTimeMultiplier()
	{
		float multiplier = magicTimeMultiplier;
		if (hasUpgrade ("magicIncreasedDistance"))
		{
			multiplier = multiplier * 1.5F;
			if (hasUpgrade ("magicResidue"))
			{
				multiplier = multiplier * 2F;
			}
		}
		return multiplier;
	}
	public virtual float getMeleeDamageMultiplier()
	{
		float multiplier = meleeDamageMultiplier;
		if (hasUpgrade("meleePlusDamage"))
		{
			multiplier = multiplier * 1.8F;
			if (hasUpgrade ("meleeMaxDamage"))
			{
				multiplier = multiplier * 1.8F;
			}
		}
		return multiplier;
	}
	public virtual float getRangedDamageMultiplier()
	{
		float multiplier = rangedDamageMultiplier;
		if (hasUpgrade("rangedPlusDamage"))
		{
			multiplier = multiplier * 1.8F;
			if (hasUpgrade ("rangedMaxDamage"))
			{
				multiplier = multiplier * 1.8F;
			}
		}
		return multiplier;
	}
	public virtual float getMagicDamageMultiplier()
	{
		float multiplier = magicDamageMultiplier;
		if (hasUpgrade("magicPlusDamage"))
		{
			multiplier = multiplier * 1.8F;
			if (hasUpgrade ("magicMaxDamage"))
			{
				multiplier = multiplier * 1.8F;
			}
		}
		return multiplier;
	}

	public bool hasUpgrade(string sub)
	{
		// Iterate through the upgrades, and return true if we find the one we want
		foreach (Upgrade upgrade in upgrades)
		{
			if (upgrade.name.Equals(sub))
			{
				return true;
			}
		}
		// If we've made it this far, the player doesn't have the upgrade
		return false;
	}
}
