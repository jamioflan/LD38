using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Entity parent;
    public float minDamage = 0.0f, maxDamage = 0.0f;
    public float cooldown = 0.5f;
    public float animTime = 0.25f;
    public float timeSinceUse = 0.0f;
    public float powerAttackDamageModifier = 2.0f;
    public float powerAttackCooldownModifier = 2.0f;
    public float powerAttackSpeedModifier = 2.0f;
    protected bool isPowerAttack = false;
    public AttackType attackType = AttackType.NOTSET;
    // Damage type
    // Debuffs
    // Etc.

	public enum AttackType
	{
		MELEE,
		RANGED,
		MAGIC,
		OTHER,
		BUNNY,
		NOTSET
	}

    public virtual void Update()
    {
        timeSinceUse += Time.deltaTime;
    }
    public virtual void Use(int attackMode, Vector2 pos, Vector2 aim)
    {

    }

    public void TryToUse(int attackMode, Vector2 pos, Vector2 aim)
    {
        if (timeSinceUse >= (isPowerAttack ? cooldown * powerAttackCooldownModifier : cooldown))
        {
            timeSinceUse = 0.0f;
            Use(attackMode, pos, aim);
        }
    }

    public virtual void UpdateAttackMove()
    {
        
    }

    public virtual float getDamageMultiplier()
	{
		return 1F;
	}

    public virtual void AttackMoveEnded()
    {
        
    }
}
