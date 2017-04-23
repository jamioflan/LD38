using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    public Projectile projectile;

    public override void Use(int attackMode, Vector2 pos, Vector2 aim)
    {
		attackType = AttackType.RANGED;

		// Note, this code is duplicated inside the RadialAttack script for the purposes of the Multiattack
        Projectile proj = Instantiate<Projectile>(projectile);
        proj.transform.position = pos;
        proj.transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Rad2Deg * Mathf.Atan2(aim.y, aim.x) - 90.0f);
        proj.attack = this;
  
        parent.SetAttackAnimState(Entity.AnimState.BOW_FIRE, animTime);

    }

	public override float getDamageMultiplier()
	{
		return parent.getRangedDamageMultiplier();
	}
}
