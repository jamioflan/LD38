﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 1.0f;

    public float timeToDeath = 10.0f;

    private Rigidbody2D rb;

    public Attack attack;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        rb.velocity = transform.up * speed;

        timeToDeath -= Time.deltaTime;
        if(timeToDeath <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if(entity == attack.parent)
        {
            return;
        }
        if(entity != null)
        {
            entity.Damage(attack);
        }

        //collision.contacts[0].normal;

        speed = 0.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        timeToDeath = 1.0f;
    }
}
