using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {
            collision.gameObject.GetComponent<Entity>().isInDoorway = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {
            collision.gameObject.GetComponent<Entity>().isInDoorway = false;
        }
    }

    public static void MoveThroughDoorway(Entity entity)
    {
        // Iterate through the doors, find out which doorway we're in, then move entity

    }
}
