using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public RoomDoor roomDoor;

	// Use this for initialization
	void Start ()
    {
        roomDoor = gameObject.GetComponentInParent<RoomDoor>();
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
            collision.gameObject.GetComponent<Entity>().doorTrigger = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {
            collision.gameObject.GetComponent<Entity>().isInDoorway = false;
            collision.gameObject.GetComponent<Entity>().doorTrigger = null;
        }
    }

    public void MoveThroughDoorway(Entity entity)
    {
        if (roomDoor != null)
        {
            RoomDoor targetDoor = roomDoor.leadsTo;
 //           entity.transform.position = targetDoor.
        }
    }
}
