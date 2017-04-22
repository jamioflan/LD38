using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2 : MonoBehaviour
{
    public enum Direction
    {
        N, W, S, E
    }

    [System.Serializable]
    public class CellInfo
    {
        public bool bRoom = false;
        public bool bNextToRoom = false;
        public bool[] abDoor = new bool[4];
        public int iColour = 0;
    }

    public readonly static int WIDTH = 16;
    public readonly static int HEIGHT = 9, MAX_ATTEMPTS = 50;
    public static readonly float tilesToWorldUnitsConversion = 2.0f;

    public Transform floorTemplate, cornerTemplate, wallTemplate, doorTemplate;

    public CellInfo[,] cells = new CellInfo[WIDTH, HEIGHT];
    public Room2 startRoom, endRoom;
    public List<Room2> rooms = new List<Room2>();

	// Use this for initialization
	void Start ()
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                startRoom.cells[i,j] = true;
                endRoom.cells[i,j] = true;
            }
        }

        ResetTiles();

        // Debug
        for (int i = 0; i < 6; i++)
            AddNewRandomRoom();
        // ----

        ShuffleRooms();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.F))
        {
            AddNewRandomRoom();

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShuffleRooms();

        }
    }

    void AddNewRandomRoom()
    {
        // Tie in random room generation. New doors are particularly lacking
        Room2 room = new Room2();
        int cellX = Random.Range(0, 3);
        int cellY = Random.Range(0, 3);
        int numCells = Random.Range(2, 18);
        for (int i = 0; i < numCells; i++)
        {
            room.cells[cellX, cellY] = true;
            if(Random.Range(0, 2) == 0)
            {
                cellX += Random.Range(-1, 2);
                if (cellX < 0) cellX = 0;
                if (cellX > 2) cellX = 2;
            }
            else
            {
                cellY += Random.Range(-1, 2);
                if (cellY < 0) cellY = 0;
                if (cellY > 2) cellY = 2;
            }
        }

        room.doors[cellX, cellY, 0] = true;

        rooms.Add(room);
        PlaceRoom(room);
    }

    void ShuffleRooms()
    {
        ResetTiles();
        PlaceRoomAt(startRoom, 0, 0);
        PlaceRoomAt(endRoom, WIDTH - 3, HEIGHT - 3);
        foreach(Room2 room in rooms)
        {
            PlaceRoom(room);
        }
        MakeGameObjects();
    }

    void ResetTiles()
    {
        for (int i = 0; i < WIDTH; i++)
        {
            for(int j = 0; j < HEIGHT; j++)
            {
                cells[i, j] = new CellInfo();
                cells[i, j].abDoor = new bool[4];
                cells[i, j].bRoom = false;
                cells[i, j].bNextToRoom = false;
                for(int k = 0; k < 4; k++)
                {
                    cells[i, j].abDoor[k] = false;
                }
                cells[i, j].iColour = 0;
            }
        }

        int numChilds = transform.childCount;
        for(int i = 0; i < numChilds; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void PlaceRoomAt(Room2 room, int x, int y)
    {
        for (int i = x; i < x + 3; i++)
        {
            for (int j = y; j < y + 3; j++)
            {
                if (room.cells[i - x, j - y])
                {
                    cells[i, j].bRoom = true;
                    for(int k = 0; k < 4; k++)
                    {
                        cells[i, j].abDoor[k] = room.doors[i - x, j - y, k];
                    }
                    for (int p = Mathf.Max(0, i - 1); p < Mathf.Min(WIDTH, i + 2); p++)
                    {
                        for (int q = Mathf.Max(0, j - 1); q < Mathf.Min(HEIGHT, j + 2); q++)
                        {
                            cells[p, q].bNextToRoom = true;
                        }
                    }
                    cells[i, j].iColour = room.colour;
                }
            }
        }
    }

    void PlaceRoom(Room2 room)
    {
        // This is the packing bit. Tie in to old packing code.
        for(int n = 0; n < MAX_ATTEMPTS; n++)
        {
            int x = Random.Range(0, WIDTH - 3);
            int y = Random.Range(0, HEIGHT - 3);
            bool bValid = true;
            for (int i = x; i < x + 3; i++)
            {
                for(int j = y; j < y + 3; j++)
                {
                    if (cells[i, j].bNextToRoom) bValid = false;
                }
            }

            if(bValid)
            {
                PlaceRoomAt(room, x, y);
                Debug.Log("Success");
                return;
            }
        }
        Debug.Log("Failure");
    }

    void MakeGameObjects()
    {
        // Floors
        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                if (cells[i, j].bRoom)
                {
                    Transform floor = Instantiate<Transform>(floorTemplate);
                    floor.SetParent(transform);
                    floor.localPosition = new Vector3(i + 0.5f, j + 0.5f, 0) * tilesToWorldUnitsConversion;
                }
            }
        }

        // Corners
        for (int i = 0; i < WIDTH + 1; i++)
        {
            for (int j = 0; j < HEIGHT + 1; j++)
            {
                int numAdjacent = 0;
                for (int x = i - 1; x < i + 1; x++)
                {
                    for (int y = j - 1; y < j + 1; y++)
                    {
                        if (x >= 0 && y >= 0 && x < WIDTH && y < HEIGHT && cells[x, y].bRoom)
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
        for (int i = 0; i < WIDTH + 1; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                bool leftAdjacent = i != 0 && cells[i - 1, j].bRoom;
                bool rightAdjacent = i != WIDTH && cells[i, j].bRoom;
                if (leftAdjacent != rightAdjacent)
                {
                    Transform wall = Instantiate<Transform>(wallTemplate);
                    wall.SetParent(transform);
                    wall.localPosition = new Vector3(i, j + 0.5f, 0) * tilesToWorldUnitsConversion;
                    if (leftAdjacent) wall.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);

                }
            }
        }

        // Walls - Horizontal
        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT + 1; j++)
            {
                bool leftAdjacent = j != 0 && cells[i, j - 1].bRoom;
                bool rightAdjacent = j != HEIGHT && cells[i, j].bRoom;
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

        // Spawn door entities
        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                for(int k = 0; k < 4; k ++)
                { 
                    if(cells[i,j].abDoor[k])
                    {
                        Transform door = Instantiate<Transform>(doorTemplate);
                        door.SetParent(transform);
                        door.localPosition = new Vector3(i + 0.5f * Mathf.Sin(k * 90.0f), j + 0.5f * Mathf.Cos(k * 90.0f), 0) * tilesToWorldUnitsConversion;
                        door.localEulerAngles = new Vector3(0.0f, 0.0f, k * 90.0f);
                    }
                }
            }
        }
    }
}
