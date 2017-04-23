using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    public Projectile projectile;
    public float attackMoveDuration = 1.0f;
    public float multiShotSpread = 15.0f;


    public override void Use(int attackMode, Vector2 pos, Vector2 aim)
    {
		attackType = AttackType.RANGED;

        switch(attackMode)
        {
                case 0:
                {
                    ShootArrows(pos, aim);
                    break;
                }
                case 1:
                {
                    parent.SetAttackMoveVector(-aim, attackMoveDuration);
                    ShootArrows(pos, aim);
                    break;
                }
        }
		

    }

    private void ShootArrows(Vector2 pos, Vector2 aim)
    {
        int numShots = parent.getRangedAttackNumShots();
        float initialAngle = Mathf.Rad2Deg * Mathf.Atan2(aim.y, aim.x) - 90.0f;
        for (int i = 0; i < numShots; i++)
        {
            float currentAngle = initialAngle + (i - numShots / 2) * multiShotSpread;
            // Note, this code is duplicated inside the RadialAttack script for the purposes of the Multiattack
            Projectile proj = Instantiate<Projectile>(projectile);
            proj.transform.position = pos;
            proj.transform.eulerAngles = new Vector3(0.0f, 0.0f, currentAngle);
            proj.attack = this;
        }

        parent.SetAttackAnimState(Entity.AnimState.BOW_FIRE, animTime);
       
    }

	public override float getDamageMultiplier()
	{
		return parent.getRangedDamageMultiplier();
	}

    public override void AttackMoveEnded()
    {

    }
}
