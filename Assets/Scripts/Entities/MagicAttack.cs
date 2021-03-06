﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : Attack
{
    public int iNumOrbs = 12;
    public float fArcBetweenOrbs = 5.0f;
    public MagicOrb orbPrefab;
	public float orbTime = 0.6F;
	public float orbDecay = 0.9F;

    public override void Use(int attackMode, Vector2 pos, Vector2 aim)
    {
        if (attackMode == 1 && !parent.hasUpgrade("magicHeavyAttack"))
            return;
        if (attackMode == 2 && !parent.hasUpgrade("magicAttackMove"))
            return;

        attackType = AttackType.MAGIC;
        switch (attackMode)
        {
            case 0:
            case 2:
                {
                    float fAimAngle = Mathf.Rad2Deg * Mathf.Atan2(aim.y, aim.x);
                    for (int i = 0; i < iNumOrbs; i++)
                    {
                        float fAngle = fAimAngle + (i - iNumOrbs / 2) * 5.0f;

                        MagicOrb orb = Instantiate<MagicOrb>(orbPrefab);
                        orb.transform.position = pos;
                        orb.transform.eulerAngles = new Vector3(0.0f, 0.0f, fAngle - 90.0f);
                        orb.attack = this;
                        orb.timeToDeath = orbTime * parent.getMagicTimeMultiplier();
                        orb.decaySpeed = 1 - (1 - orbDecay) / parent.getMagicDistanceMultiplier();

                        
                    }

                    isPowerAttack = attackMode == 2;

                    parent.SetAttackAnimState(Entity.AnimState.BOW_FIRE, animTime * (isPowerAttack ? powerAttackSpeedModifier : 1.0f));
                    break;
                }
            case 1:
                {
                    Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
                    RaycastHit2D hit2D = Physics2D.GetRayIntersection(new Ray(pos, aim));
                    if (hit2D)
                    {
                        rb.MovePosition(transform.position + (Vector3)aim.normalized * (hit2D.distance - 0.25f));
                    }
                    else rb.MovePosition(pos + aim);

                    float fAimAngle = Mathf.Rad2Deg * Mathf.Atan2(aim.y, aim.x);

                    for (int i = 0; i < iNumOrbs; i++)
                    {
                        Vector2 startPos = pos + Random.insideUnitCircle * 1.0f;

                        MagicOrb orb = Instantiate<MagicOrb>(orbPrefab);
                        orb.transform.position = startPos;
                        orb.transform.eulerAngles = new Vector3(0.0f, 0.0f, fAimAngle - 90.0f);
                        orb.attack = this;
                        orb.timeToDeath = orbTime * parent.getMagicTimeMultiplier();
                        orb.decaySpeed = 1 - (1 - orbDecay) / parent.getMagicDistanceMultiplier();

                        
                    }

                    parent.SetAttackAnimState(Entity.AnimState.BOW_FIRE, animTime);

                    break;
                }
        }

    }

	public override float getDamageMultiplier()
	{
		return parent.getMagicDamageMultiplier() * (isPowerAttack ? powerAttackDamageModifier : 1);
	}

    public override void AttackMoveEnded()
    {

    }

}
