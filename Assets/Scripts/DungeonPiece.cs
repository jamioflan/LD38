using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiece : MonoBehaviour {

    public static readonly float tilesToWorldUnitsConversion = 2.0f;

    public PositionedRoom positionedRoom;
    public Transform floorTemplate, cornerTemplate, wallTemplate, doorTemplate;

	// Use this for initialization
	void Start ()
    {
        GenerateEdges();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(positionedRoom != null)
        {
            if (positionedRoom.pos == null)
            {
                transform.position = new Vector3(100.0f, 0.0f, 0.0f);
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                transform.position = new Vector3(positionedRoom.pos.x, positionedRoom.pos.y, 0) * tilesToWorldUnitsConversion;
                transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f * positionedRoom.rotation);
            }
        }	
        
	}

    void GenerateEdges()
    {
        if(positionedRoom == null)
        {
            Debug.Assert(false, "Positioned room for DungeonPiece is null!");
        }

        // Floors
        for(int i = 0; i < RoomShape.maxMatrixWidth; i++)
        {
            for(int j = 0; j < RoomShape.maxMatrixHeight; j++)
            {
                if(positionedRoom.room.matrix[i,j] != null)
                {
                    Transform floor = Instantiate<Transform>(floorTemplate);
                    floor.SetParent(transform);
                    floor.localPosition = new Vector3(i + 0.5f, j + 0.5f, 0) * tilesToWorldUnitsConversion;
                }
            }
        }

        // Corners
        for (int i = 0; i < RoomShape.maxMatrixWidth + 1; i++)
        {
            for (int j = 0; j < RoomShape.maxMatrixHeight + 1; j++)
            {
                int numAdjacent = 0;
                for(int x = i - 1; x < i + 1; x++)
                {
                    for(int y = j - 1; y < j + 1; y++)
                    {
                        if(x >= 0 && y >= 0 && x < RoomShape.maxMatrixWidth && y < RoomShape.maxMatrixHeight && positionedRoom.room.matrix[x, y] != null)
                        {
                            numAdjacent++;
                        }
                    }
                }
                if (numAdjacent > 0 && numAdjacent < 4)
                {
                    Transform corner = Instantiate<Transform>(cornerTemplate);
                    corner.SetParent(transform);
                    corner.localPosition = new Vector3(i, j, 0) * tilesToWorldUnitsConversion;
                }
            }
        }

        // Walls - Vertical
        for (int i = 0; i < RoomShape.maxMatrixWidth + 1; i++)
        {
            for (int j = 0; j < RoomShape.maxMatrixHeight; j++)
            {
                bool leftAdjacent = i != 0 && positionedRoom.room.matrix[i - 1, j] != null;
                bool rightAdjacent = i != RoomShape.maxMatrixWidth && positionedRoom.room.matrix[i, j] != null;
                if (leftAdjacent != rightAdjacent)
                {
                    Transform wall = Instantiate<Transform>(doorTemplate);
                    wall.SetParent(transform);
                    wall.localPosition = new Vector3(i, j + 0.5f, 0) * tilesToWorldUnitsConversion;
                    if(leftAdjacent) wall.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);

                }
            }
        }

        // Walls - Horizontal
        for (int i = 0; i < RoomShape.maxMatrixWidth; i++)
        {
            for (int j = 0; j < RoomShape.maxMatrixHeight + 1; j++)
            {
                bool leftAdjacent = j != 0 && positionedRoom.room.matrix[i, j - 1] != null;
                bool rightAdjacent = j != RoomShape.maxMatrixHeight && positionedRoom.room.matrix[i, j] != null;
                if (leftAdjacent != rightAdjacent)
                {
                    // TODO - Doors!
                    Transform wall = Instantiate<Transform>(wallTemplate);
                    wall.SetParent(transform);
                    wall.localPosition = new Vector3(i + 0.5f, j, 0) * tilesToWorldUnitsConversion;
                    if (leftAdjacent) wall.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                    else wall.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
                }
            }
        }
    }
}
