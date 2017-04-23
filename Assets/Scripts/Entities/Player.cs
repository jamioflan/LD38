using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public float maxSpeed = 10F;
    public List<Upgrade> upgrades = new List<Upgrade>();

    public bool switchWeapon = false, switchWeaponLast = false;

    public Transform crosshairObject;

    public override void Awake()
    {
        base.Awake();

        Game.thePlayer = this;

        healthRegenRate = 0.1f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rigidBody.velocity = move * maxSpeed;

        

        if(move.sqrMagnitude > 0.1f)
        {
            SetAnimState(AnimState.WALKING);
        }
        else
        {
            SetAnimState(AnimState.IDLE);
        }

        Vector3 mouse = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        crosshair = mouse;

        crosshairObject.position = crosshair;

        if (Input.GetAxis("Fire1") > 0)
        {
            UseCurrentAttack(0);
        }
        if (Input.GetAxis("Fire2") > 0)
        {
            UseCurrentAttack(1);
        }
        if (Input.GetAxis("Fire3") > 0)
        {
            UseCurrentAttack(2);
        }

        switchWeaponLast = switchWeapon;
        switchWeapon = Input.GetAxis("Jump") > 0;

        if (switchWeapon && !switchWeaponLast)
        {
            SelectWeapon((currentAttack + 1) % 2);
        }
    }

    public void SelectWeapon(int weapon)
    {
        attacks[currentAttack].gameObject.SetActive(false);
        currentAttack = weapon;
        attacks[currentAttack].gameObject.SetActive(true);
    }

	public override float getAttackArcMultiplier()
	{
		float multiplier = attackArcMultiplier;
		if (hasUpgrade("meleeIncreasedArc"))
		{
			multiplier = multiplier * 2;
			if (hasUpgrade ("melee360"))
			{
				multiplier = multiplier * 2;
			}
		}
		return multiplier;
	}

	public bool hasUpgrade(string sub)
	{
		// Iterate through the upgrades, and return true if we find the one we want
		foreach (Upgrade upgrade in upgrades)
		{
			if (upgrade.name.Equals(sub))
			{
				return true;
			}
		}
		// If we've made it this far, the player doesn't have the upgrade
		return false;
	}
}