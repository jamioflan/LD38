using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    public static readonly int MAX_ROOMS = 100;

    public RoomShape[] roomShapes;
    public int numRooms = 0;
    public PositionedRoom[] currentLayout;
    public PositionedRoom[] nextLayout;

    // Use this for initialization
    void Start ()
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        // Hacky way to trigger it
        if(Input.GetKeyDown(KeyCode.G))
        {
            generateShapes(10);
        }
    }

    public void generateShapes(int num)
    {
        this.roomShapes = new RoomShape[MAX_ROOMS];
        for (int i=0; i<num; i++)
        {
            int size = Random.Range(2, 10);
            RoomShape roomShape = new RoomShape();
            roomShape.addBlock(0,0);
            int blocksMade = 1;
            int loopsDone = 0;
            while(blocksMade < size && loopsDone++<1000)
            {
                int x = Random.Range(0, RoomShape.maxMatrixWidth - 1);
                int y = Random.Range(0, RoomShape.maxMatrixHeight - 1);
                if (roomShape.matrix[x, y] == null) continue;
                RoomBlock roomBlock = roomShape.matrix[x, y];
                RoomShape.Position[] adjacentEmpties = roomBlock.getAdjacentEmptyPositions();
                int numEmpties = 0;
                for (int j=0; j<4; j++)
                {
                    if (adjacentEmpties[j] != null) numEmpties++;
                }
                if (numEmpties == 0) continue;
                RoomShape.Position nextPos = adjacentEmpties[Random.Range(0, numEmpties - 1)];
                roomShape.addBlock(nextPos.x, nextPos.y);
                blocksMade++;
            }
            Debug.Assert(loopsDone < 1000, "Infinite shape generation loop detected");
            this.roomShapes[i] = roomShape;
        }
        this.numRooms = num;
    }

    public void generateNextLayout()
    {
        this.nextLayout = new PositionedRoom[MAX_ROOMS];
        for (int i=0; i<this.numRooms; i++)
        {
            PositionedRoom.Position pos = new PositionedRoom.Position(i * 5, 0);
            nextLayout[i] = new PositionedRoom(this.roomShapes[i], pos, 0);
        }
    }

    public void updateToNextLayout()
    {
        this.currentLayout = this.nextLayout;
        this.nextLayout = null;
    }


}
