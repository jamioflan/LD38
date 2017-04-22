using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float maxSpeed = 10F;

	void Start ()
	{
		
	}

	void FixedUpdate ()
	{

		Rigidbody2D rigidBody = GetComponent<Rigidbody2D> ();

		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");

		rigidBody.velocity = new Vector2(moveX * maxSpeed, moveY * maxSpeed);
	}
}