using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicOrb : MonoBehaviour
{
    public float speed = 1.0f;

    public float timeToDeath = 0.6f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float aoeRange = 0.1f;
    public float aoeInterval = 1.0f;
    public float timeSinceLastAOE = 0.0f;
	public float decaySpeed = 0.9F;

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
		speed *= decaySpeed;
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
}
