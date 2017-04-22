using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomBlock
{

    public RoomShape parentRoomShape;

    public struct Walls
    {
        public RoomWall north;
        public RoomWall west;
        public RoomWall south;
        public RoomWall east;
    }
    public Walls walls;

    RoomShape.Position pos;

    public RoomBlock(RoomShape parent, int x, int y)
    {
        this.parentRoomShape = parent;
        this.pos = new RoomShape.Position(x,y);
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

}