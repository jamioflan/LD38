using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Entity parent;
    public float damage;
    // Damage type
    // Debuffs
    // Etc.

    public virtual void Use(int attackMode, Vector2 pos, Vector2 aim)
    {

    }
}
