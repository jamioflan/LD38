using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Entity target;

    public enum EnemyState
    {
        IDLE,
        ATTACKING
    }

    public EnemyState state = EnemyState.IDLE;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
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
                    target = null;
                }
                break;
            }
        }

        if(target != null)
        {
            MoveToTarget();
            crosshair = target.transform.position;
            UseCurrentAttack(0);
        }
    }

    private void MoveToTarget()
    {
        Vector3 dPos = (target.transform.position - transform.position).normalized;
        rb.velocity = dPos * moveSpeed;
    }
}
