using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialAttack : Attack {

    public float range = 1.0f;
    public float basehalfarc = Mathf.PI / 2;
	public Projectile projectile;

    public override void Use(int attackMode, Vector2 pos, Vector2 aim)
    {
		attackType = AttackType.MELEE;

        foreach (Collider2D coll in Physics2D.OverlapCircleAll(pos, range))
        {
            Entity entity = coll.GetComponent<Entity>();
			float halfarc = basehalfarc * parent.getAttackArcMultiplier();
            Vector3 dPos = coll.transform.position - (Vector3)pos;
            if(entity != null && entity != parent && Mathf.Acos(Vector3.Dot(dPos.normalized, aim.normalized)) < halfarc)
            {
                entity.Damage(this);
            }
        }

        parent.SetAttackAnimState(parent.isPlayer ? Entity.AnimState.SWORD_SLASH : Entity.AnimState.LUNGE, animTime);

		if (parent.isPlayer && ((Player)parent).hasUpgrade("meleeRangedMultiattack"))
			{
			// Note, this is a direct copy of the Rnaged Attack code, and should be edited with care
				Projectile proj = Instantiate<Projectile>(projectile);
				proj.transform.position = pos;
				proj.transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Rad2Deg * Mathf.Atan2(aim.y, aim.x) - 90.0f);
				proj.attack = this;
			}
    }

	public override float getDamageMultiplier()
	{
		return parent.getMeleeDamageMultiplier();
	}
}
