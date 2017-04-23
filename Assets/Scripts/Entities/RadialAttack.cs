using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialAttack : Attack {

    public float range = 1.0f;
    public float basehalfarc = Mathf.PI / 2;
	public Projectile projectile;
    public float attackMoveDuration = 0.3f;

    public override void Use(int attackMode, Vector2 pos, Vector2 aim)
    {
        if (attackMode == 1 && !parent.hasUpgrade("meleePowerAttack"))
            return;
        if (attackMode == 2 && !parent.hasUpgrade("meleeAttackMove"))
            return;

        switch (attackMode)
        {
                case 0: // ATTACK
                case 2:
                {
                    attackType = AttackType.MELEE;
                    isPowerAttack = attackMode == 2;

                    foreach (Collider2D coll in Physics2D.OverlapCircleAll(pos, range))
                    {
                        Entity entity = coll.GetComponent<Entity>();
                        float halfarc = basehalfarc * parent.getAttackArcMultiplier();
                        Vector3 dPos = coll.transform.position - (Vector3)pos;
                        if (entity != null && entity != parent && Mathf.Acos(Vector3.Dot(dPos.normalized, aim.normalized)) < halfarc)
                        {
                            entity.Damage(this);
                        }
                    }

                    parent.SetAttackAnimState(parent.isPlayer ? Entity.AnimState.SWORD_SLASH : Entity.AnimState.LUNGE, animTime * (isPowerAttack ? powerAttackSpeedModifier : 1.0f));

                    if (parent.hasUpgrade("meleeRangedMultiattack"))
                    {
                        // Note, this is a direct copy of the Ranged Attack code, and should be edited with care
                        Projectile proj = Instantiate<Projectile>(projectile);
                        proj.transform.position = pos;
                        proj.transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Rad2Deg * Mathf.Atan2(aim.y, aim.x) - 90.0f);
                        proj.attack = this;
                    }
                    break;
                }
                case 1: // ATTACK MOVE
                {
                    parent.SetAttackMoveVector(aim, attackMoveDuration);
                    entitiesHit.Clear();
                    break;
                }
        }
		
    }

    public override void Update()
    {
        base.Update();
    }

    private List<Entity> entitiesHit = new List<Entity>();

    public override void UpdateAttackMove()
    {
        foreach (Collider2D coll in Physics2D.OverlapCircleAll(parent.transform.position, range))
        {
            Entity entity = coll.GetComponent<Entity>();
            float halfarc = basehalfarc * parent.getAttackArcMultiplier();
            Vector3 dPos = coll.transform.position - (Vector3)parent.transform.position;
            if (entity != null && entity != parent && Mathf.Acos(Vector3.Dot(dPos.normalized, parent.attackMoveVector.normalized)) < halfarc)
            {
                if(!entitiesHit.Contains(entity))
                {
                    entitiesHit.Add(entity);
                    entity.Damage(this);
                }
            }
        }
    }

    public override float getDamageMultiplier()
	{
		return parent.getMeleeDamageMultiplier() * (isPowerAttack ? powerAttackDamageModifier : 1);
	}

    public override void AttackMoveEnded()
    {
        foreach (Collider2D coll in Physics2D.OverlapCircleAll(parent.transform.position, range))
        {
            Entity entity = coll.GetComponent<Entity>();
            float halfarc = basehalfarc * parent.getAttackArcMultiplier();
            Vector3 dPos = coll.transform.position - (Vector3)parent.transform.position;
            if (entity != null && entity != parent && Mathf.Acos(Vector3.Dot(dPos.normalized, parent.attackMoveVector.normalized)) < halfarc)
            {
                entity.Damage(this);
            }
        }
    }
}
