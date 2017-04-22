using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Attack[] attacks;
    public int currentAttack;
    public float maxHealth = 10.0f;
    public float health = 10.0f;

    public int facing = 0;
    public Sprite body, leftHand, rightHand;

    public float maxInvulnerabilityCooldown = 1.0f;
    public float invulnerabilityCooldown = 1.0f;

    public Vector2 target;

    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
        invulnerabilityCooldown -= Time.deltaTime;

    }

    protected void SetFacing(int dir)
    {
        facing = dir;
        body.
    }

    protected void UseCurrentAttack(int attackMode)
    {
        attacks[currentAttack].Use(attackMode, transform.position, (Vector3)target - transform.position);
    }

    public void Damage(Attack attack)
    {
        if(invulnerabilityCooldown <= 0.0f)
        {
            // Do damage;
            invulnerabilityCooldown = maxInvulnerabilityCooldown;
            health -= attack.damage;
        }
       
    }
}
