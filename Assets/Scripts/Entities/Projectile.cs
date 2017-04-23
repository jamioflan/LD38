using System.Collections;
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

        if(true)//attack.parent.isBouncy())
        {
            float fNormalAngle = Mathf.Rad2Deg * Mathf.Atan2(collision.contacts[0].normal.y, collision.contacts[0].normal.x);
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, 2.0f * fNormalAngle - transform.localEulerAngles.z);
        }
        else
        {
            speed = 0.0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            timeToDeath = 1.0f;
        }


        if (entity != null)
        {
            entity.Damage(attack);
            timeToDeath = 0.0f;
        }
    }
}
