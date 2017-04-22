using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public float maxSpeed = 10F;
    public List<Upgrade> upgrades = new List<Upgrade>();

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
    }
}