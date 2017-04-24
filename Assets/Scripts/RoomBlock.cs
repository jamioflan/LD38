using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomBlock
{

    public RoomShape parentRoomShape;
    
    public RoomWall[] walls;

    RoomShape.Position pos;

    public RoomBlock(RoomShape parent, int x, int y)
    {
        this.parentRoomShape = parent;
        this.pos = new RoomShape.Position(x,y);
        this.walls = new RoomWall[4];
    }

    public RoomBlock[] getAdjacentBlocks()
    {
        RoomBlock[] adjacents = new RoomBlock[4];
        int indexAt = 0;
        if (this.pos.x > 0 && this.parentRoomShape.matrix[this.pos.x - 1, this.pos.y] != null)
        {
            adjacents[indexAt++] = this.parentRoomShape.matrix[this.pos.x - 1, this.pos.y];
        }
        if (this.pos.y > 0 && this.parentRoomShape.matrix[this.pos.x, this.pos.y - 1] != null)
        {
            adjacents[indexAt++] = this.parentRoomShape.matrix[this.pos.x, this.pos.y - 1];
        }
        if (this.pos.x < RoomShape.maxMatrixWidth && this.parentRoomShape.matrix[this.pos.x + 1, this.pos.y] != null)
        {
            adjacents[indexAt++] = this.parentRoomShape.matrix[this.pos.x + 1, this.pos.y];
        }
        if (this.pos.y < RoomShape.maxMatrixHeight && this.parentRoomShape.matrix[this.pos.x, this.pos.y + 1] != null)
        {
            adjacents[indexAt++] = this.parentRoomShape.matrix[this.pos.x, this.pos.y + 1];
        }
        return adjacents;
    }

    public RoomShape.Position[] getAdjacentEmptyPositions()
    {
        RoomShape.Position[] empties = new RoomShape.Position[4];
        int indexAt = 0;
        if (this.pos.x > 0 && this.parentRoomShape.matrix[this.pos.x - 1, this.pos.y] == null)
        {
            empties[indexAt++] = new RoomShape.Position(this.pos.x - 1, this.pos.y);
        }
        if (this.pos.y > 0 && this.parentRoomShape.matrix[this.pos.x, this.pos.y - 1] == null)
        {
            empties[indexAt++] = new RoomShape.Position(this.pos.x, this.pos.y - 1);
        }
        if (this.pos.x < RoomShape.maxMatrixWidth && this.parentRoomShape.matrix[this.pos.x + 1, this.pos.y] == null)
        {
            empties[indexAt++] = new RoomShape.Position(this.pos.x + 1, this.pos.y);
        }
        if (this.pos.y < RoomShape.maxMatrixHeight && this.parentRoomShape.matrix[this.pos.x, this.pos.y + 1] == null)
        {
            empties[indexAt++] = new RoomShape.Position(this.pos.x, this.pos.y + 1);
        }
        return empties;
    }

}

public class RoomWall
{

    public RoomBlock parentRoomBlock;
    public RoomShape parentRoomShape
    {
        get { return this.parentRoomBlock.parentRoomShape; }
    }

    public RoomDoor door;

    public WallDoor wallDoor;

    public RoomWall(RoomBlock parent)
    {
        this.parentRoomBlock = parent;
    }

}

public class RoomDoor
{

    public RoomWall parentRoomWall;
    public RoomBlock parentRoomBlock
    {
        get { return this.parentRoomWall.parentRoomBlock; }
    }
    public RoomShape parentRoomShape
    {
        get { return this.parentRoomBlock.parentRoomShape; }
    }

    public RoomDoor leadsTo;
    public DoorTrigger myTrigger = null;

    public RoomDoor(RoomWall parent)
    {
        this.parentRoomWall = parent;
    }

}