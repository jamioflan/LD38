using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    public SpriteRenderer[] numbers = new SpriteRenderer[4];
    public Sprite[] sprites = new Sprite[10];

    public float life = 1.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        life -= Time.deltaTime;
        transform.position += new Vector3(0.0f, 0.5f * Time.deltaTime, 0.0f);
        if(life <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetNumber(int n)
    {
        for(int i = 0; i < 4; i++)
        {
            if (n == 0)
            {
                numbers[3 - i].enabled = false;
            }
            numbers[3 - i].sprite = sprites[n % 10];
            n /= 10;
        }
    }
}
