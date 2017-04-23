using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : Attack
{
    public int iNumOrbs = 12;
    public float fArcBetweenOrbs = 5.0f;
    public MagicOrb orbPrefab;

    public override void Use(int attackMode, Vector2 pos, Vector2 aim)
    {
        float fAimAngle = Mathf.Rad2Deg * Mathf.Atan2(aim.y, aim.x);
        for(int i = 0; i < iNumOrbs; i++)
        {
            float fAngle = fAimAngle + (i - iNumOrbs / 2) * 5.0f;

            MagicOrb orb = Instantiate<MagicOrb>(orbPrefab);
            orb.transform.position = pos;
            orb.transform.eulerAngles = new Vector3(0.0f, 0.0f, fAngle - 90.0f);
            orb.attack = this;

            parent.SetAttackAnimState(Entity.AnimState.BOW_FIRE, animTime);
        }

      
    }
}
