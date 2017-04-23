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

    public void Update()
    {
        timeSinceUse += Time.deltaTime;
    }
    public virtual void Use(int attackMode, Vector2 pos, Vector2 aim)
    {

    }

    public void TryToUse(int attackMode, Vector2 pos, Vector2 aim)
    {
        if (timeSinceUse >= cooldown)
        {
            timeSinceUse = 0.0f;
            Use(attackMode, pos, aim);
        }
    }

	public virtual float getDamageMultiplier()
	{
		return 1F;
	}

    public virtual void AttackMoveEnded()
    {
        
    }
}
