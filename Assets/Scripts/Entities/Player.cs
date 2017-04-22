using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public float maxSpeed = 10F;
    public int XP = 0;
    public List<Upgrade> upgrades = new List<Upgrade>();

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

        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rigidBody.velocity = move * maxSpeed;

        Vector3 mouse = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        crosshair = mouse;

        crosshairObject.position = crosshair;
    }
}