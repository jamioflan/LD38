using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    public static readonly int MAX_ROOMS = 3;

    public static readonly int G_LAYOUT_IT = 3; // The number of attempts the algorithm makes at generating a good layout
    public static readonly int G_SHAPE_POS_IT = 100; // The number of attempts the generation algorithm makes when placing a shape

    public RoomShape[] roomShapes;
    public GoldRoomShape goldRoomA;
    public GoldRoomShape goldRoomB;
    public int numRooms = 0;
    public PositionedRoom[] currentLayout;
    public PositionedRoom[] nextLayout;

    public DungeonPiece pieceTemplate;

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
            generateShapes(MAX_ROOMS);
            generateNextLayout();
            updateToNextLayout();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(""+this.getLayoutIslands(this.currentLayout));
            RoomBlock[,] matrix = this.getLayoutBlockMatrix(this.currentLayout);
            for (int y = Grid.instance.height-1; y >= 0 ; y--)
            {
                string line = "";
                for (int x = 0; x < Grid.instance.width; x++)
                {
                    if (matrix[x, y] != null) line += "1";
                    else line += "0";
                }
                Debug.Log(line);
            }
        }
    }

    private RoomShape createShape()
    {
        RoomShape roomShape = new RoomShape();
        PositionedRoom PosRoom = new PositionedRoom(roomShape);
        DungeonPiece newPiece = Instantiate<DungeonPiece>(pieceTemplate);
        newPiece.positionedRoom = PosRoom;
        roomShape.dungeonPiece = newPiece;
        return roomShape;
    }

    public void generateShapes(int num)
    {
        this.roomShapes = new RoomShape[MAX_ROOMS];
        this.createCreateGoldShapes();
        this.currentLayout = null;
        this.nextLayout = null;
        for (int i=2; i<num; i++)
        {
            int size = Random.Range(2, 10);
            RoomShape roomShape = this.createShape();
            int startx = Random.Range(0, RoomShape.maxMatrixWidth);
            int starty = Random.Range(0, RoomShape.maxMatrixHeight);
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
            roomShape.generateWalls();
            foreach (RoomBlock block in roomShape.matrix)
            {
                if (block != null)
                {
                    for (int w = 0; w < 4; w++)
                    {
                        if (block.walls[w] != null)
                        {
                            if (Random.Range(0, 1) < 0.5) block.walls[w].door = new RoomDoor(block.walls[w]);
                        }
                    }
                }
            }
            this.roomShapes[i] = roomShape;
        }
        for (int i = 0; i < num; i++)
        {
            foreach (RoomBlock block in this.roomShapes[i].matrix)
            {
                if (block != null)
                {
                    for (int w = 0; w < 4; w++)
                    {
                        if (block.walls[w] != null && block.walls[w].door != null)
                        {
                            int loopsDone = 0;
                            RoomWall wall = null;
                            do
                            {
                                int index = Random.Range(0, num);
                                RoomShape otherShape = this.roomShapes[index];
                                RoomWall[] walls = otherShape.getWalls();
                                int numWalls = Utilities.nonNullLen<RoomWall>(walls);
                                if (numWalls == 0) continue;
                                int wallIndex = Random.Range(0, numWalls);
                                wall = walls[wallIndex];
                                if (wall.door != null) break;
                            } while (loopsDone++ < 1000);
                            Debug.Assert(loopsDone < 1000, "Infinite loop detected in wall connection generation.");
                            if (loopsDone >= 1000) continue;
                            block.walls[w].door.leadsTo = wall.door;
                        }
                    }
                }
            }
        }
        this.numRooms = num;
    }

    private void createCreateGoldShapes()
    {
        this.goldRoomA = new GoldRoomShape();
        this.goldRoomB = new GoldRoomShape();
        roomShapes[0] = this.goldRoomA;
        roomShapes[1] = this.goldRoomB;
        this.goldRoomA.generateWalls();
        this.goldRoomB.generateWalls();
        PositionedRoom posRoomA = new PositionedRoom(this.goldRoomA);
        PositionedRoom posRoomB = new PositionedRoom(this.goldRoomB);
        DungeonPiece pieceA = Instantiate<DungeonPiece>(pieceTemplate);
        DungeonPiece pieceB = Instantiate<DungeonPiece>(pieceTemplate);
        pieceA.positionedRoom = posRoomA;
        pieceB.positionedRoom = posRoomB;
        this.goldRoomA.dungeonPiece = pieceA;
        this.goldRoomB.dungeonPiece = pieceB;
    }

    public void generateNextLayout()
    {
        this.nextLayout = new PositionedRoom[MAX_ROOMS];
        PositionedRoom[] attemptLayout = new PositionedRoom[MAX_ROOMS];
        int[] complexities = new int[this.numRooms];
        int[] shuffledIndicies = new int[this.numRooms];
        for (int i = 0; i < this.numRooms; i++)
        {
            complexities[i] = this.roomShapes[i].getComplexity();
            shuffledIndicies[i] = i;
            attemptLayout[i] = new PositionedRoom(this.roomShapes[i]);
        }
        System.Array.Sort(complexities);
        Utilities.shuffle<int>(shuffledIndicies);
        attemptLayout[0].pos = new Grid.Position(0, 0);
        attemptLayout[1].pos = new Grid.Position(Grid.instance.width-GoldRoomShape.WIDTH, 0);
        for (int j = 0; j < G_LAYOUT_IT; j++)
        {
            for (int i = 0; i < this.numRooms; i++)
            {
                attemptLayout[i].pos = null;
            }
            foreach (int complexity in complexities)
            {
                foreach (int index in shuffledIndicies)
                {
                    if (!(this.roomShapes[index] is GoldRoomShape) && this.roomShapes[index].getComplexity() == complexity)
                    {
                        PositionedRoom positionedRoom = attemptLayout[index];
                        int bestNiceness;
                        if (positionedRoom.pos == null)
                        {
                            bestNiceness = -1;
                        }
                        else
                        {
                            bestNiceness = this.getLayoutNiceness(attemptLayout);
                        }
                        int x;
                        int y;
                        int rotation;
                        Grid.Position pos;
                        PositionedRoomSkel bestshapePos = new PositionedRoomSkel(positionedRoom.pos, 0);
                        for (int i = 0; i < G_SHAPE_POS_IT; i++)
                        {
                            int loopsDone = 0;
                            do
                            {
                                rotation = Random.Range(0, 4);
                                rotation = 0; // TODO
                                positionedRoom.rotation = rotation;
                                positionedRoom.calculateBounds();
                                x = Random.Range(0 - positionedRoom.bounds.minX, Grid.instance.width - positionedRoom.bounds.maxX);
                                y = Random.Range(0 - positionedRoom.bounds.minY, Grid.instance.height - positionedRoom.bounds.maxY);
                                pos = new Grid.Position(x, y);
                                positionedRoom.pos = pos;
                                if (!positionedRoom.collides(attemptLayout)) break;
                            } while (loopsDone++ < 1000);
                            Debug.Assert(loopsDone < 1000, "Tried a lot of ways to position a shape; none of which worked. Hmmm.");
                            if (loopsDone >= 1000) continue;
                            int niceness = this.getLayoutNiceness(attemptLayout);
                            if (niceness > bestNiceness)
                            {
                                bestshapePos.pos = pos;
                                bestshapePos.rotation = rotation;
                                bestNiceness = niceness;
                            }
                        }
                        positionedRoom.createFromSkel(bestshapePos);
                    }
                }
            }
            // int layoutNiceness = this.getLayoutNiceness(attemptLayout);
            // if (layoutNiceness > bestLayoutNiceness)
            // {
            //     System.Array.Copy(attemptLayout, this.nextLayout, MAX_ROOMS);
            //     bestLayoutNiceness = layoutNiceness;
            // }
        }
        this.nextLayout = attemptLayout;
    }

    private int getLayoutNiceness(PositionedRoom[] layout)
    {
        int minX = Grid.instance.width;
        int minY = Grid.instance.height;
        int maxX = 0;
        int maxY = 0;
        int numBlocks = 0;
        foreach (PositionedRoom positionedRoom in layout)
        {
            if (positionedRoom != null && positionedRoom.pos != null)
            {
                positionedRoom.calculateBounds();
                int absMinX = positionedRoom.bounds.minX + positionedRoom.pos.x;
                int absMinY = positionedRoom.bounds.minY + positionedRoom.pos.y;
                int absMaxX = positionedRoom.bounds.maxX + positionedRoom.pos.x;
                int absMaxY = positionedRoom.bounds.maxY + positionedRoom.pos.y;
                if (absMinX < minX) minX = absMinX;
                if (absMinY < minY) minY = absMinY;
                if (absMaxX > maxX) maxX = absMaxX;
                if (absMaxY > maxY) maxY = absMaxY;
                numBlocks += positionedRoom.room.getNumBlocks();
            }
        }
        float numIslands = getLayoutIslands(layout);
        float nf = 100f * (float)numBlocks / (float)((maxX - minX + 1) * (maxX - minX + 1) + (maxY - minY + 1) * (maxY - minY + 1)) / numIslands;
        return Mathf.FloorToInt(nf);

    }

    private RoomBlock[,] getLayoutBlockMatrix(PositionedRoom[] layout)
    {
        RoomBlock[,] blockMatrix = new RoomBlock[Grid.instance.width, Grid.instance.height];
        Grid.Position pos = new Grid.Position(0,0);
        for (int x = 0; x < Grid.instance.width; x++)
        {
            pos.x = x;
            for (int y = 0; y < Grid.instance.height; y++)
            {
                pos.y = y;
                foreach (PositionedRoom positionedRoom in layout)
                {
                    if (positionedRoom != null && positionedRoom.pos != null)
                    {
                        RoomBlock block = positionedRoom.getRoomBlock(pos);
                        if (block != null) blockMatrix[x, y] = block;
                    }
                }
            }
        }
        return blockMatrix;
    }

    private int getLayoutIslands(PositionedRoom[] layout)
    {
        RoomBlock[,] blockMatrix = this.getLayoutBlockMatrix(layout);
        int[,] islandCodes = new int[Grid.instance.width, Grid.instance.height];
        int numIslands = 0;
        for (int x = 0; x < Grid.instance.width; x++)
        {
            for (int y = 0; y < Grid.instance.height; y++)
            {
                if (islandCodes[x, y] == 0 && blockMatrix[x, y] != null)
                {
                    numIslands++;
                    islandCodes[x, y] = numIslands;
                    bool changesMade;
                    do
                    {
                        changesMade = false;
                        for (int nx = 0; nx < Grid.instance.width; nx++)
                        {
                            for (int ny = 0; ny < Grid.instance.height; ny++)
                            {
                                if (islandCodes[nx, ny] == 0 && blockMatrix[nx,ny] != null)
                                {
                                    if (nx > 0 && islandCodes[nx - 1, ny] == numIslands) { islandCodes[nx, ny] = numIslands; changesMade = true; }
                                    if (ny > 0 && islandCodes[nx, ny - 1] == numIslands) { islandCodes[nx, ny] = numIslands; changesMade = true; }
                                    if (nx < Grid.instance.width - 1 && islandCodes[nx + 1, ny] == numIslands) { islandCodes[nx, ny] = numIslands; changesMade = true; }
                                    if (ny < Grid.instance.height - 1 && islandCodes[nx, ny + 1] == numIslands) { islandCodes[nx, ny] = numIslands; changesMade = true; }
                                }
                            }
                        }
                    } while (changesMade);
                }
            }
        }
        return numIslands;
    }

    public void updateToNextLayout()
    {
        this.currentLayout = this.nextLayout;
        for (int i = 0; i < this.numRooms; i++)
        {
            DungeonPiece dungeonPiece = this.roomShapes[i].dungeonPiece;
            dungeonPiece.positionedRoom = this.currentLayout[i];
            dungeonPiece.updatePiece();
        }
        this.nextLayout = null;
    }


}
