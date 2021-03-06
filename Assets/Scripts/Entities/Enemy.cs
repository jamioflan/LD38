﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Entity target;

    public float minAttackRange = 0.0f, maxAttackRange = 2.0f;

    public enum EnemyState
    {
        IDLE,
        ATTACKING
    }

    public EnemyState state = EnemyState.IDLE;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
	}

    // Update is called once per frame
    public override void FixedUpdate ()
    {
        base.FixedUpdate();

        switch(state)
        {
            case EnemyState.IDLE:
            {
                if (Game.thePlayer.currentRoom == currentRoom)
                {
                    state = EnemyState.ATTACKING;
                    target = Game.thePlayer;
                }
                break;
            }
            case EnemyState.ATTACKING:
            {
                if (Game.thePlayer.currentRoom != currentRoom)
                {
                    state = EnemyState.IDLE;
                    if(rb != null)
                        rb.velocity = new Vector2(0.0f, 0.0f);
                    target = null;
                }
                break;
            }
        }

        if(target != null)
        {
            float fDist = (new Vector2(transform.position.x - target.transform.position.x, transform.position.y - target.transform.position.y)).magnitude;

            if(fDist > minAttackRange)
            {
                MoveToTarget();
                SetAnimState(AnimState.WALKING);
            }
            else rb.velocity = Vector3.zero; 

            if (fDist <= maxAttackRange)
            {
                crosshair = target.transform.position;
                UseCurrentAttack(0);
            }
        }
        else
        {
            SetAnimState(AnimState.IDLE);

        }
    }

    private void MoveToTarget()
    {
        if (rb == null) return;

        Vector3 dPos = (target.transform.position - transform.position).normalized;

		if (bleedtimer > 0)
		{
			rb.velocity = dPos * moveSpeed / 2;
		}
		else
		{
			rb.velocity = dPos * moveSpeed;
		}
    }
}
