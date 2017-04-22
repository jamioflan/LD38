using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialAttack : Attack {

    public float range = 1.0f;
    public float arc = Mathf.PI;

    public override void Use(int attackMode, Vector2 pos, Vector2 aim)
    {
        foreach (Collider2D coll in Physics2D.OverlapCircleAll(pos, range))
        {
            Entity entity = coll.GetComponent<Entity>();
            Vector3 dPos = coll.transform.position - (Vector3)pos;
            if(entity != null && entity != parent && Mathf.Acos(Vector3.Dot(dPos.normalized, aim.normalized)) < arc)
            {
                entity.Damage(this);
            }
        }



    }
}
