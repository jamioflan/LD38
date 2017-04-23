using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialAttack : Attack {

    public float range = 1.0f;
    public float basehalfarc = Mathf.PI / 2;

    public override void Use(int attackMode, Vector2 pos, Vector2 aim)
    {
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

        parent.SetAttackAnimState(Entity.AnimState.SWORD_SLASH, animTime);

    }
}
