﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public bool switchWeapon = false, switchWeaponLast = false;
    public bool enterDoor = false, enterDoorLast = false;

    public int numRoomsIn = 0;


    public Transform crosshairObject;

    public override void Awake()
    {
        base.Awake();

        Game.thePlayer = this;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        if(numRoomsIn <= 0)
        {
            transform.position = (new Vector2(-12.89f, -1.69f));
        }

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

		Vector3 mouse = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
		crosshair = mouse;

        if(move.sqrMagnitude > 0.1f)
        {
            SetAnimState(AnimState.WALKING);

			if (hasUpgrade("meleeMagicTrails") && timeSinceLastFlamingStep > 0.4F)
			{
				timeSinceLastFlamingStep += Time.fixedDeltaTime;
				if (timeSinceLastFlamingStep > 0.4F)
				{
					timeSinceLastFlamingStep = 0F;
					MagicOrb orb = Instantiate<MagicOrb> (orbPrefab);
					orb.transform.position = transform.position + new Vector3 (0F, -0.3F, 0F);
					orb.transform.eulerAngles = new Vector3 (0F, 0F, Mathf.Rad2Deg * Mathf.Atan2 (move.y, move.x) + 90.0f);
					orb.attack = attacks [2];
					orb.timeToDeath = orbTime * getMagicTimeMultiplier ();
					orb.decaySpeed = orbDecay;
				}
			}
        }
        else
        {
            SetAnimState(AnimState.IDLE);
        }

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

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            SelectWeapon((currentAttack + 1) % 3);
        }

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			SelectWeapon((currentAttack + 2) % 3);
		}

        enterDoorLast = enterDoor;
        enterDoor = Input.GetAxis("Open Door") > 0.0f;

        // Check if we want to go through a doorway
        if (isInDoorway && enterDoor && !enterDoorLast)
        {
            doorTrigger.MoveThroughDoorway(this);
        }
    }

    public void SelectWeapon(int weapon)
    {
        attacks[currentAttack].gameObject.SetActive(false);
        currentAttack = weapon;
        attacks[currentAttack].gameObject.SetActive(true);
    }
}