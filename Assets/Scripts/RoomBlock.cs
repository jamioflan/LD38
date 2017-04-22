using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBlock
{

    public RoomShape parentRoomShape;

    public class Walls
    {
        public RoomWall north;
        public RoomWall west;
        public RoomWall south;
        public RoomWall east;
    }

}

public class RoomWall
{

    public RoomBlock parentRoomBlock;

    public RoomDoor door;

    public RoomShape parentRoomShape()
    {
        return this.parentRoomBlock.parentRoomShape;
    }

}

public class RoomDoor
{

    public RoomWall parentRoomWall;

    public RoomDoor leadsTo;

    public RoomBlock parentRoomBlock()
    {
        return this.parentRoomWall.parentRoomBlock;
    }

    public RoomShape parentRoomShape()
    {
        return this.parentRoomBlock().parentRoomShape;
    }

}