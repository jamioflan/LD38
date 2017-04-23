using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public RoomWall roomWall;

	// Use this for initialization
	void Start ()
    {
        roomWall = gameObject.GetComponentInParent<RoomWall>();
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
        if (roomWall != null && roomWall.door != null)
        {
            // We're gonna get a loop here with bosses! They'll flicker between two rooms. Needs fixing.

            RoomDoor targetDoor = roomWall.door.leadsTo;
            int wallIndexThisDoorIsOn = 0;

            for (int iIndex = 0; iIndex < 4; iIndex++)
            {
                RoomWall wall = targetDoor.parentRoomBlock.walls[iIndex];
                if (wall != null && wall.door == targetDoor)
                {
                    wallIndexThisDoorIsOn = (iIndex + targetDoor.parentRoomWall.wallDoor.positionedRoom.rotation) % 4;
                }
            }

            Vector3 offset;
            switch (wallIndexThisDoorIsOn)
            {
                case 0:
                default:
                {
                    offset = new Vector3(0.0f, -DungeonPiece.tilesToWorldUnitsConversion, 0.0f);
                    break;
                }
                case 1:
                {
                    offset = new Vector3(DungeonPiece.tilesToWorldUnitsConversion, 0.0f, 0.0f);
                    break;
                }
                case 2:
                {
                    offset = new Vector3(0.0f, DungeonPiece.tilesToWorldUnitsConversion, 0.0f);
                    break;
                }
                case 3:
                {
                    offset = new Vector3(-DungeonPiece.tilesToWorldUnitsConversion, 0.0f, 0.0f);
                    break;
                }
            }

            entity.transform.position = targetDoor.parentRoomWall.wallDoor.transform.position + offset;
        }
    }
}
