using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Vector2 targetPos;

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
	void Update ()
    {
        if(Game.thePlayer.currentRoom == currentRoom)
        {
            targetPos = Game.thePlayer.transform.position;
        }

    }
}
