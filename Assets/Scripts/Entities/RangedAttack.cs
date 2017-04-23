using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    public Projectile projectile;

    public override void Use(int attackMode, Vector2 pos, Vector2 aim)
    {
        Projectile proj = Instantiate<Projectile>(projectile);
        proj.transform.position = pos;
        proj.transform.LookAt(pos + aim);
  
        parent.SetAttackAnimState(Entity.AnimState.BOW_FIRE, animTime);

    }
}
