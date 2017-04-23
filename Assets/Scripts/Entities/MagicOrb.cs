using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicOrb : MonoBehaviour
{
    public float speed = 1.0f;

    public float timeToDeath = 5.5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float aoeRange = 0.1f;
    public float aoeInterval = 1.0f;
    public float timeSinceLastAOE = 0.0f;

    public Attack attack;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed *= 0.90f;
        rb.velocity = transform.up * speed;

        if (Random.Range(0, 30) == 0)
            sr.flipX = !sr.flipX;
        if (Random.Range(0, 30) == 0)
            sr.flipY = !sr.flipY;

        timeToDeath -= Time.deltaTime;
        if (timeToDeath <= 0.0f)
        {
            Destroy(gameObject);
        }

        timeSinceLastAOE += Time.deltaTime;
        if (timeSinceLastAOE > aoeInterval)
        {
            timeSinceLastAOE = 0.0f;
            foreach (Collider2D coll in Physics2D.OverlapCircleAll(transform.position, aoeRange))
            {
                Entity entity = coll.GetComponent<Entity>();
                if (entity != null && entity != attack.parent)
                {
                    entity.Damage(attack);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity == attack.parent)
        {
            return;
        }
        if (entity != null)
        {
            entity.Damage(attack);
        }

        //collision.contacts[0].normal;

        speed = 0.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        timeToDeath = 1.0f;
    }
}
