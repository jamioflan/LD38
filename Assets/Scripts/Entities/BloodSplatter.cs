using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{

    public float timeToDeath = 0.6f;

    private SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
		if (Random.Range(0, 2) == 0)
			sr.flipX = !sr.flipX;
		if (Random.Range(0, 2) == 0)
			sr.flipY = !sr.flipY;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeToDeath -= Time.deltaTime;
        if (timeToDeath <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
