using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

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

        if (attackMoveTimer > 0.0f)
        {
            attackMoveTimer -= Time.fixedDeltaTime;
            move = attackMoveVector;

            attacks[currentAttack].UpdateAttackMove();

            if (attackMoveTimer <= 0.0f)
            {
                // Move ended
                attacks[currentAttack].AttackMoveEnded();
            }
        }

		if (bleedtimer > 0)
		{
			rigidBody.velocity = move * moveSpeed / 2;
		}
		else
		{
			rigidBody.velocity = move * moveSpeed;
		}

        

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
        switchWeapon = Input.GetAxis("Mouse ScrollWheel") > 0;

        if (switchWeapon && !switchWeaponLast)
        {
            SelectWeapon((currentAttack + 1) % 3);
        }
			
		switchWeapon = Input.GetAxis("Mouse ScrollWheel") < 0;

		if (switchWeapon && !switchWeaponLast)
		{
			SelectWeapon((currentAttack + 2) % 3);
		}
    }

    public void SelectWeapon(int weapon)
    {
        attacks[currentAttack].gameObject.SetActive(false);
        currentAttack = weapon;
        attacks[currentAttack].gameObject.SetActive(true);
    }
}