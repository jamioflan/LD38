using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    public static readonly int MAX_ROOMS = 20;

    public static readonly int G_LAYOUT_IT = 3; // The number of attempts the algorithm makes at generating a good layout
    public static readonly int G_SHAPE_POS_IT = 100; // The number of attempts the generation algorithm makes when placing a shape

    public static readonly int START_ROOMS = 7;
    public static readonly int ADVANCE_NEW_ROOMS = 2;
    public static readonly int EXTRA_DOOR_LINKS_P = 1; // The number of extra door links to add per room, in addition to the base connecting links. 

    public int roomsInUse;

    public RoomShape[] roomShapes;
    public GoldRoomShape goldRoomA;
    public GoldRoomShape goldRoomB;
    public int numRooms = 0;
    public PositionedRoom[] currentLayout;
    public PositionedRoom[] nextLayout;

    public DungeonPiece pieceTemplate;

    // Use this for initialization
    void Awake ()
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

    public RoomController()
    {
        this.roomsInUse = START_ROOMS;
    }

    public void advanceLevel()
    {
        this.roomsInUse += ADVANCE_NEW_ROOMS;
        if (this.roomsInUse > MAX_ROOMS) this.roomsInUse = MAX_ROOMS;
        this.generateNextLayout();
        this.updateToNextLayout();
    }

    private RoomShape createShape()
    {
        RoomShape roomShape = new RoomShape();
        PositionedRoom PosRoom = new PositionedRoom(roomShape);
        DungeonPiece newPiece = Instantiate<DungeonPiece>(pieceTemplate);
        newPiece.positionedRoom = PosRoom;
        newPiece.transform.SetParent(transform);
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
            int size = UnityEngine.Random.Range(2, 10);
            RoomShape roomShape = this.createShape();
            int startx = UnityEngine.Random.Range(0, RoomShape.maxMatrixWidth);
            int starty = UnityEngine.Random.Range(0, RoomShape.maxMatrixHeight);
            roomShape.addBlock(0,0);
            int blocksMade = 1;
            int loopsDone = 0;
            while(blocksMade < size && loopsDone++<1000)
            {
                int x = UnityEngine.Random.Range(0, RoomShape.maxMatrixWidth - 1);
                int y = UnityEngine.Random.Range(0, RoomShape.maxMatrixHeight - 1);
                if (roomShape.matrix[x, y] == null) continue;
                RoomBlock roomBlock = roomShape.matrix[x, y];
                RoomShape.Position[] adjacentEmpties = roomBlock.getAdjacentEmptyPositions();
                int numEmpties = 0;
                for (int j=0; j<4; j++)
                {
                    if (adjacentEmpties[j] != null) numEmpties++;
                }
                if (numEmpties == 0) continue;
                RoomShape.Position nextPos = adjacentEmpties[UnityEngine.Random.Range(0, numEmpties - 1)];
                roomShape.addBlock(nextPos.x, nextPos.y);
                blocksMade++;
            }
            Debug.Assert(loopsDone < 1000, "Infinite shape generation loop detected");
            roomShape.generateWalls();
            //foreach (RoomBlock block in roomShape.matrix)
            //{
            //    if (block != null)
            //    {
            //        for (int w = 0; w < 4; w++)
            //        {
            //            if (block.walls[w] != null)
            //            {
            //                if (Random.Range(0, 1) < 0.5) block.walls[w].door = new RoomDoor(block.walls[w]);
            //            }
            //        }
            //    }
            //}
            this.roomShapes[i] = roomShape;
        }

        // for (int i = 0; i < num; i++)
        // {
        //     foreach (RoomBlock block in this.roomShapes[i].matrix)
        //     {
        //         if (block != null)
        //         {
        //             for (int w = 0; w < 4; w++)
        //             {
        //                 if (block.walls[w] != null && block.walls[w].door != null)
        //                 {
        //                     int loopsDone = 0;
        //                     RoomWall wall = null;
        //                     do
        //                     {
        //                         int index = Random.Range(0, num);
        //                         RoomShape otherShape = this.roomShapes[index];
        //                         RoomWall[] walls = otherShape.getWalls();
        //                         int numWalls = Utilities.nonNullLen<RoomWall>(walls);
        //                         if (numWalls == 0) continue;
        //                         int wallIndex = Random.Range(0, numWalls);
        //                         wall = walls[wallIndex];
        //                         if (wall.door != null) break;
        //                     } while (loopsDone++ < 1000);
        //                     Debug.Assert(loopsDone < 1000, "Infinite loop detected in wall connection generation.");
        //                     if (loopsDone >= 1000) continue;
        //                     block.walls[w].door.leadsTo = wall.door;
        //                 }
        //             }
        //         }
        //     }
        // }
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
        pieceA.transform.SetParent(transform);
        pieceB.transform.SetParent(transform);
        pieceA.positionedRoom = posRoomA;
        pieceB.positionedRoom = posRoomB;
        this.goldRoomA.dungeonPiece = pieceA;
        this.goldRoomB.dungeonPiece = pieceB;
    }

    public void generateNextLayout()
    {
        this.nextLayout = new PositionedRoom[MAX_ROOMS];
        PositionedRoom[] attemptLayout = new PositionedRoom[MAX_ROOMS];
        int[] complexities = new int[this.roomsInUse];
        int[] shuffledIndicies = new int[this.roomsInUse];
        for (int i = 0; i < this.roomsInUse; i++)
        {
            complexities[i] = this.roomShapes[i].getComplexity();
            shuffledIndicies[i] = i;
            attemptLayout[i] = new PositionedRoom(this.roomShapes[i]);
        }
        System.Array.Sort(complexities);
        Utilities.shuffle<int>(shuffledIndicies);
        attemptLayout[0].pos = new Grid.Position(0, 2);
        attemptLayout[1].pos = new Grid.Position(Grid.instance.width-GoldRoomShape.WIDTH, 2);
        for (int j = 0; j < G_LAYOUT_IT; j++)
        {
            //for (int i = 0; i < this.roomsInUse; i++)
            //{
            //    attemptLayout[i].pos = null;
            //}
            foreach (int complexity in complexities)
            {
                foreach (int index in shuffledIndicies)
                {
                    if (!(this.roomShapes[index] is GoldRoomShape) && this.roomShapes[index].getComplexity() == complexity)
                    {
                        PositionedRoom positionedRoom = attemptLayout[index];
                        int bestNiceness;
                        PositionedRoomSkel bestshapePos;
                        if (positionedRoom.pos == null)
                        {
                            bestNiceness = -1;
                            bestshapePos = new PositionedRoomSkel(positionedRoom.pos, 0);
                        }
                        else
                        {
                            bestNiceness = this.getLayoutNiceness(attemptLayout);
                            bestshapePos = new PositionedRoomSkel(positionedRoom.pos, positionedRoom.rotation);
                        }
                        int x;
                        int y;
                        int rotation;
                        Grid.Position pos;
                        for (int i = 0; i < G_SHAPE_POS_IT; i++)
                        {
                            int loopsDone = 0;
                            do
                            {
                                rotation = UnityEngine.Random.Range(0, 4);
                                positionedRoom.rotation = rotation;
                                positionedRoom.calculateBounds();
                                x = UnityEngine.Random.Range(0 - positionedRoom.bounds.minX, Grid.instance.width - positionedRoom.bounds.maxX);
                                y = UnityEngine.Random.Range(0 - positionedRoom.bounds.minY, Grid.instance.height - positionedRoom.bounds.maxY);
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
                                positionedRoom.createFromSkel(bestshapePos);
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

        for (int i =0; i<this.roomsInUse; i++)
        {
            RoomWall[] walls = this.roomShapes[i].getWalls();
            foreach (RoomWall w in walls)
            {
                if (w != null)
                {
                    w.door = null;
                }
            }
        }
        
        bool[] inTree = new bool[this.roomsInUse];
        for (int i = 0; i < this.roomsInUse - 1; i++)
        {
            int loopsDone = 0;
            do
            {
                int v1;
                int v2;
                do
                {
                    v1 = UnityEngine.Random.Range(0, this.roomsInUse);
                    v2 = UnityEngine.Random.Range(0, this.roomsInUse);
                } while (v1 == v2 || (i > 0 && (inTree[v1] || !inTree[v2])));
                RoomWall w1 = this.roomShapes[v1].randomUndooredWall();
                RoomWall w2 = this.roomShapes[v2].randomUndooredWall();
                if (w1 == null) continue;
                if (w2 == null) continue;
                w1.door = new RoomDoor(w1);
                w2.door = new RoomDoor(w2);
                w1.door.leadsTo = w2.door;
                w2.door.leadsTo = w1.door;
                inTree[v1] = true;
                inTree[v2] = true;
                break;
            } while (loopsDone++ < 1000);
            Debug.Assert(loopsDone < 1000, "Infinite Loop!");
        }

        // TODO Prevent the boss room from getting extra links
        for (int i = 0; i < EXTRA_DOOR_LINKS_P * this.roomsInUse; i++)
        {
            int loopsDone = 0;
            do
            {
                int v1 = UnityEngine.Random.Range(2, this.roomsInUse);
                int v2 = UnityEngine.Random.Range(2, this.roomsInUse);
                if (v1 == v2) continue;
                RoomWall w1 = this.roomShapes[v1].randomUndooredWall();
                RoomWall w2 = this.roomShapes[v2].randomUndooredWall();
                if (w1 == null) continue;
                if (w2 == null) continue;
                w1.door = new RoomDoor(w1);
                w2.door = new RoomDoor(w2);
                w1.door.leadsTo = w2.door;
                w2.door.leadsTo = w1.door;
                break;
            } while (loopsDone++ < 1000);
            Debug.Assert(loopsDone < 1000, "Infinite Loop!");
        }
            
    }

    private int getLayoutNiceness(PositionedRoom[] layout)
    {
        int minX = Grid.instance.width;
        int minY = Grid.instance.height;
        int maxX = 0;
        int maxY = 0;
        int totalX = 0;
        //int totalY = 0;
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
                totalX += positionedRoom.pos.x;
                //totalY += positionedRoom.pos.y;
                numBlocks += positionedRoom.room.getNumBlocks();
            }
        }
        float numIslands = getLayoutIslands(layout);
        float avgX =  (float)totalX / (float)(numBlocks * Grid.instance.width);
        //float avgY = (float)totalY / (float)(numBlocks * Grid.instance.height);
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
        for (int i = 0; i < this.roomsInUse; i++)
        {
            DungeonPiece dungeonPiece = this.roomShapes[i].dungeonPiece;
            dungeonPiece.positionedRoom = this.currentLayout[i];
            dungeonPiece.updatePiece();
        }
        this.nextLayout = null;
    }


}
