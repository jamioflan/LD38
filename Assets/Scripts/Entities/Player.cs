using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public float maxSpeed = 10F;
    public int XP = 0;
    public List<Upgrade> upgrades = new List<Upgrade>();

    public override void Awake()
    {
        base.Awake();

        Game.thePlayer = this;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rigidBody.velocity = new Vector2(moveX * maxSpeed, moveY * maxSpeed);
    }
}