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

		Vector3 mouse = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
		crosshair = mouse;

		timeSinceLastFlamingStep += Time.fixedDeltaTime;

        if(move.sqrMagnitude > 0.1f)
        {
            SetAnimState(AnimState.WALKING);

			if (hasUpgrade("meleeMagicTrails") && timeSinceLastFlamingStep > 0.4F)
			{
				timeSinceLastFlamingStep = 0F;
				MagicOrb orb = Instantiate<MagicOrb>(orbPrefab);
				orb.transform.position = transform.position + new Vector3(0F, -0.3F, 0F);
				orb.transform.eulerAngles = new Vector3(0F, 0F, Mathf.Rad2Deg * Mathf.Atan2(move.y, move.x) + 90.0f);
				orb.attack = attacks[2];
				orb.timeToDeath = orbTime * getMagicTimeMultiplier();
				orb.decaySpeed = orbDecay;
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