using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiece : MonoBehaviour {

    public static readonly float tilesToWorldUnitsConversion = 2.0f;
    public static readonly float wallOffsetTiles = 0.0f;//0.05f;

    public PositionedRoom positionedRoom;
    public Transform cornerTemplate, wallTemplate, doorTemplate;
    public FloorPiece floorTemplate;

    public List<Enemy> pendingEnemies = new List<Enemy>();

    public List<FloorPiece> floorPieces = new List<FloorPiece>();

    public WallDoor wallDoorTemplate;

    public float complexity;

    public Boss boss = null;

    // Use this for initialization
    void Start ()
    {
        GenerateEdges();
    }

    // Update is called once per frame
    void Update()
    {
        updatePiece();

        if(this.positionedRoom.pos == null)
        {
            return;
        }
        while (pendingEnemies.Count > 0)

        {
            
            FloorPiece location = floorPieces[Random.Range(0, floorPieces.Count)];
            pendingEnemies[pendingEnemies.Count - 1].transform.position = location.transform.position;// + (Vector3)Random.insideUnitCircle;
            pendingEnemies[pendingEnemies.Count - 1].gameObject.SetActive(true);
            pendingEnemies[pendingEnemies.Count - 1].currentRoom = this;
            pendingEnemies.RemoveAt(pendingEnemies.Count - 1);
        }


    }

    public int inRoomCount = 0;
    public void EnteredRoom()
    {
        if (inRoomCount == 0)
        {
            foreach(FloorPiece floor in floorPieces)
            {
                floor.SetHidden(0);
            }
        }

        Game.thePlayer.currentRoom = this;

        if(boss != null)
            boss.ActivateBoss();

        inRoomCount++;
    }
    public void ExitedRoom()
    {
        inRoomCount--;
        if (inRoomCount == 0)
        {
            foreach (FloorPiece floor in floorPieces)
            {
                floor.SetHidden(1);
            }
        }
    }

    public void ForceExitedRoom()
    {
        foreach (FloorPiece floor in floorPieces)
        {
            floor.SetHidden(2);
        }
    }

    public void updatePiece()
    {
        if (this.positionedRoom != null)
        {
            this.complexity = this.positionedRoom.room.getComplexity();
            if (this.positionedRoom.pos == null)
            {
                transform.localPosition = new Vector3(100.0f, 0.0f, 0.0f);
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                transform.localPosition = new Vector3(this.positionedRoom.pos.x, this.positionedRoom.pos.y, 0) * tilesToWorldUnitsConversion;
                transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f * this.positionedRoom.rotation);
            }
        }

    }


    public void GenerateEdges()
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
                    FloorPiece floor = Instantiate<FloorPiece>(floorTemplate);
                    floor.transform.SetParent(transform);
                    floor.transform.localPosition = new Vector3(i + 0f, j + 0f, 0) * tilesToWorldUnitsConversion;
                    floor.piece = this;
                    floorPieces.Add(floor);
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
                    corner.localPosition = new Vector3(i - 0.5f, j - 0.5f, 0) * tilesToWorldUnitsConversion;
                }
            }
        }

        for (int i = 0; i < RoomShape.maxMatrixWidth; i++)
        {
            for (int j = 0; j < RoomShape.maxMatrixHeight; j++)
            {
                if (positionedRoom.room.matrix[i, j] != null)
                {
                    if (positionedRoom.room.matrix[i, j].walls[0] != null)
                    {
                        Transform wall = null;
                        if (positionedRoom.room.matrix[i, j].walls[0].door == null)
                        {
                            wall = Instantiate<Transform>(wallTemplate);
                        }
                        else
                        {
                            wall = Instantiate<Transform>(doorTemplate);
                            wall.GetComponentInChildren<DoorTrigger>().roomWall = positionedRoom.room.matrix[i, j].walls[0];
                            positionedRoom.room.matrix[i, j].walls[0].door.myTrigger = wall.GetComponentInChildren<DoorTrigger>();
                        }
                        wall.SetParent(transform);
                        wall.localPosition = new Vector3(i, j + 0.5f - wallOffsetTiles, 0) * tilesToWorldUnitsConversion;
                        wall.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);

                    }
                    if (positionedRoom.room.matrix[i, j].walls[1] != null)
                    {
                        Transform wall = null;
                        if (positionedRoom.room.matrix[i, j].walls[1].door == null)
                        {
                            wall = Instantiate<Transform>(wallTemplate);
                        }
                        else
                        {
                            wall = Instantiate<Transform>(doorTemplate);
                            wall.GetComponentInChildren<DoorTrigger>().roomWall = positionedRoom.room.matrix[i, j].walls[1];
                            positionedRoom.room.matrix[i, j].walls[1].door.myTrigger = wall.GetComponentInChildren<DoorTrigger>();
                        }
                        wall.SetParent(transform);
                        wall.localPosition = new Vector3(i - 0.5f + wallOffsetTiles, j, 0) * tilesToWorldUnitsConversion;
                        wall.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    }
                    if (positionedRoom.room.matrix[i, j].walls[2] != null)
                    {
                        Transform wall = null;
                        if (positionedRoom.room.matrix[i, j].walls[2].door == null)
                        {
                            wall = Instantiate<Transform>(wallTemplate);
                        }
                        else
                        {
                            wall = Instantiate<Transform>(doorTemplate);
                            wall.GetComponentInChildren<DoorTrigger>().roomWall = positionedRoom.room.matrix[i, j].walls[2];
                            positionedRoom.room.matrix[i, j].walls[2].door.myTrigger = wall.GetComponentInChildren<DoorTrigger>();
                        }
                        wall.SetParent(transform);
                        wall.localPosition = new Vector3(i, j - 0.5f + wallOffsetTiles, 0) * tilesToWorldUnitsConversion;
                        wall.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                    }
                    if (positionedRoom.room.matrix[i, j].walls[3] != null)
                    {
                        Transform wall = null;
                        if (positionedRoom.room.matrix[i, j].walls[3].door == null)
                        {
                            wall = Instantiate<Transform>(wallTemplate);
                        }
                        else
                        {
                            wall = Instantiate<Transform>(doorTemplate);
                            wall.GetComponentInChildren<DoorTrigger>().roomWall = positionedRoom.room.matrix[i, j].walls[3];
                            positionedRoom.room.matrix[i, j].walls[3].door.myTrigger = wall.GetComponentInChildren<DoorTrigger>();
                        }
                        wall.SetParent(transform);
                        wall.localPosition = new Vector3(i + 0.5f - wallOffsetTiles, j, 0) * tilesToWorldUnitsConversion;
                        wall.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                    }
                }
            }
        }

        // for (int i = 0; i < RoomShape.maxMatrixWidth; i++)
        // {
        //     for (int j = 0; j < RoomShape.maxMatrixHeight; j++)
        //     {
        //         if (positionedRoom.room.matrix[i, j] != null)
        //         {
        //             if (positionedRoom.room.matrix[i, j].walls[0] != null)
        //             {
        //                 if (positionedRoom.room.matrix[i, j].walls[0].door == null)
        //                 {
        //                     Transform wall = Instantiate<Transform>(wallTemplate);
        //                     wall.SetParent(transform);
        //                     wall.localPosition = new Vector3(i, j + 0.5f - wallOffsetTiles, 0) * tilesToWorldUnitsConversion;
        //                     wall.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
        //                 }
        //                 else
        //                 {
        //                     WallDoor wall = Instantiate<WallDoor>(wallDoorTemplate);
        //                     wall.transform.SetParent(transform);
        //                     wall.transform.localPosition = new Vector3(i, j + 0.5f - wallOffsetTiles, 0) * tilesToWorldUnitsConversion;
        //                     wall.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
        //                     wall.roomWall = positionedRoom.room.matrix[i, j].walls[0];
        //                     wall.positionedRoom = positionedRoom;
        //                     positionedRoom.room.matrix[i, j].walls[0].wallDoor = wall;
        //                 }
        //             }
        //             if (positionedRoom.room.matrix[i, j].walls[1] != null)
        //             {
        //                 if (positionedRoom.room.matrix[i, j].walls[1].door == null)
        //                 {
        //                     Transform wall = Instantiate<Transform>(wallTemplate);
        //                     wall.SetParent(transform);
        //                     wall.localPosition = new Vector3(i - 0.5f + wallOffsetTiles, j, 0) * tilesToWorldUnitsConversion;
        //                     wall.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
        //                 }
        //                 else
        //                 {
        //                     WallDoor wall = Instantiate<WallDoor>(wallDoorTemplate);
        //                     wall.transform.SetParent(transform);
        //                     wall.transform.localPosition = new Vector3(i - 0.5f + wallOffsetTiles, j, 0) * tilesToWorldUnitsConversion;
        //                     wall.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
        //                     wall.roomWall = positionedRoom.room.matrix[i, j].walls[1];
        //                     wall.positionedRoom = positionedRoom;
        //                     positionedRoom.room.matrix[i, j].walls[1].wallDoor = wall;
        //                 }
        //                 // Transform wall;
        //                 // if (positionedRoom.room.matrix[i, j].walls[1].door == null)
        //                 //     wall = Instantiate<Transform>(wallTemplate);
        //                 // else
        //                 //     wall = Instantiate<Transform>(doorTemplate);
        //                 // wall.SetParent(transform);
        //                 // wall.localPosition = new Vector3(i - 0.5f + wallOffsetTiles, j, 0) * tilesToWorldUnitsConversion;
        //                 // wall.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
        //             }
        //             if (positionedRoom.room.matrix[i, j].walls[2] != null)
        //             {
        //                 if (positionedRoom.room.matrix[i, j].walls[2].door == null)
        //                 {
        //                     Transform wall = Instantiate<Transform>(wallTemplate);
        //                     wall.SetParent(transform);
        //                     wall.localPosition = new Vector3(i, j - 0.5f + wallOffsetTiles, 0) * tilesToWorldUnitsConversion;
        //                     wall.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
        //                 }
        //                 else
        //                 {
        //                     WallDoor wall = Instantiate<WallDoor>(wallDoorTemplate);
        //                     wall.transform.SetParent(transform);
        //                     wall.transform.localPosition = new Vector3(i, j - 0.5f + wallOffsetTiles, 0) * tilesToWorldUnitsConversion;
        //                     wall.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
        //                     wall.roomWall = positionedRoom.room.matrix[i, j].walls[2];
        //                     wall.positionedRoom = positionedRoom;
        //                     positionedRoom.room.matrix[i, j].walls[2].wallDoor = wall;
        //                 }
        //                 // Transform wall;
        //                 // if (positionedRoom.room.matrix[i, j].walls[2].door == null)
        //                 //     wall = Instantiate<Transform>(wallTemplate);
        //                 // else
        //                 //     wall = Instantiate<Transform>(doorTemplate);
        //                 // wall.SetParent(transform);
        //                 // wall.localPosition = new Vector3(i, j - 0.5f + wallOffsetTiles, 0) * tilesToWorldUnitsConversion;
        //                 // wall.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
        //             }
        //             if (positionedRoom.room.matrix[i, j].walls[3] != null)
        //             {
        //                 if (positionedRoom.room.matrix[i, j].walls[3].door == null)
        //                 {
        //                     Transform wall = Instantiate<Transform>(wallTemplate);
        //                     wall.SetParent(transform);
        //                     wall.localPosition = new Vector3(i + 0.5f - wallOffsetTiles, j, 0) * tilesToWorldUnitsConversion;
        //                     wall.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        //                 }
        //                 else
        //                 {
        //                     WallDoor wall = Instantiate<WallDoor>(wallDoorTemplate);
        //                     wall.transform.SetParent(transform);
        //                     wall.transform.localPosition = new Vector3(i + 0.5f - wallOffsetTiles, j, 0) * tilesToWorldUnitsConversion;
        //                     wall.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        //                     wall.roomWall = positionedRoom.room.matrix[i, j].walls[3];
        //                     wall.positionedRoom = positionedRoom;
        //                     positionedRoom.room.matrix[i, j].walls[3].wallDoor = wall;
        //                 }
        //                 // Transform wall;
        //                 // if (positionedRoom.room.matrix[i, j].walls[3].door == null)
        //                 //     wall = Instantiate<Transform>(wallTemplate);
        //                 // else
        //                 //     wall = Instantiate<Transform>(doorTemplate);
        //                 // wall.SetParent(transform);
        //                 // wall.localPosition = new Vector3(i + 0.5f - wallOffsetTiles, j, 0) * tilesToWorldUnitsConversion;
        //                 // wall.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        //             }
        //         }
        //     }
        // }
        
         // // Walls - Vertical
         // for (int i = 0; i < RoomShape.maxMatrixWidth + 1; i++)
         // {
         //     for (int j = 0; j < RoomShape.maxMatrixHeight; j++)
         //     {
         //         bool leftAdjacent = i != 0 && positionedRoom.room.matrix[i - 1, j] != null;
         //         bool rightAdjacent = i != RoomShape.maxMatrixWidth && positionedRoom.room.matrix[i, j] != null;
         //         if (leftAdjacent != rightAdjacent)
         //         {
         //             Transform wall = Instantiate<Transform>(doorTemplate);
         //             wall.SetParent(transform);
         //             wall.localPosition = new Vector3(i - 0.5f, j + 0.0f, 0) * tilesToWorldUnitsConversion;
         //             if (leftAdjacent) wall.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
         // 
         //         }
         //     }
         // }
         // 
         // // Walls - Horizontal
         // for (int i = 0; i < RoomShape.maxMatrixWidth; i++)
         // {
         //     for (int j = 0; j < RoomShape.maxMatrixHeight + 1; j++)
         //     {
         //         bool leftAdjacent = j != 0 && positionedRoom.room.matrix[i, j - 1] != null;
         //         bool rightAdjacent = j != RoomShape.maxMatrixHeight && positionedRoom.room.matrix[i, j] != null;
         //         if (leftAdjacent != rightAdjacent)
         //         {
         //             // TODO - Doors!
         //             Transform wall = Instantiate<Transform>(wallTemplate);
         //             wall.SetParent(transform);
         //             wall.localPosition = new Vector3(i + 0f, j - 0.5f, 0) * tilesToWorldUnitsConversion;
         //             if (leftAdjacent) wall.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
         //             else wall.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
         //         }
         //     }
         // }
    }
}

public class WallDoor : MonoBehaviour
{

    public RoomWall roomWall;

    public PositionedRoom positionedRoom;

    public Transform wallTemplate, doorTemplate;

    // Use this for initialization
    void Start()
    {
        Transform wall = Instantiate<Transform>(doorTemplate);
        wall.SetParent(transform);
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(0f,0f,0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

}