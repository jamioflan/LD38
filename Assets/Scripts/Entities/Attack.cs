using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Entity parent;
    public float damage;
    public float cooldown = 0.5f;
    public float timeSinceUse = 0.0f;
    // Damage type
    // Debuffs
    // Etc.

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
}
