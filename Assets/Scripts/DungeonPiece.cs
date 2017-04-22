using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiece : MonoBehaviour {

    public static readonly float tilesToWorldUnitsConversion = 2.0f;

    public RoomShape shape;
    public Transform floorTemplate, cornerTemplate, wallTemplate, doorTemplate;

	// Use this for initialization
	void Start ()
    {
        shape = RoomShapeTemplates.test;
        GenerateEdges();

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void GenerateEdges()
    {
        if(shape == null)
        {
            Debug.Assert(false, "Shape is null!");
        }

        // Floors
        for(int i = 0; i < RoomShape.maxMatrixWidth; i++)
        {
            for(int j = 0; j < RoomShape.maxMatrixHeight; j++)
            {
                if(shape.matrix[i,j] != null)
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
                        if(x >= 0 && y >= 0 && x < RoomShape.maxMatrixWidth && y < RoomShape.maxMatrixHeight && shape.matrix[x, y] != null)
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
                int numAdjacent = 0;
                if (i != 0 && shape.matrix[i - 1, j] != null) numAdjacent++;
                if (i != RoomShape.maxMatrixWidth && shape.matrix[i, j] != null) numAdjacent++;
                if (numAdjacent == 1)
                {
                    // TODO - Doors!
                    Transform wall = Instantiate<Transform>(wallTemplate);
                    wall.SetParent(transform);
                    wall.localPosition = new Vector3(i, j + 0.5f, 0) * tilesToWorldUnitsConversion;
                }
            }
        }

        // Walls - Horizontal
        for (int i = 0; i < RoomShape.maxMatrixWidth; i++)
        {
            for (int j = 0; j < RoomShape.maxMatrixHeight + 1; j++)
            {
                int numAdjacent = 0;
                if (j != 0 && shape.matrix[i, j - 1] != null) numAdjacent++;
                if (j != RoomShape.maxMatrixHeight && shape.matrix[i, j] != null) numAdjacent++;
                if (numAdjacent == 1)
                {
                    // TODO - Doors!
                    Transform wall = Instantiate<Transform>(wallTemplate);
                    wall.SetParent(transform);
                    wall.localPosition = new Vector3(i + 0.5f, j, 0) * tilesToWorldUnitsConversion;
                    wall.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                }
            }
        }

    }
}
