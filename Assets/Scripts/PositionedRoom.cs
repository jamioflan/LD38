using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionedRoom
{

    public RoomShape room;
    public int rotation; // Rotation: anticlockwise in steps of 90deg
    public class Position
    {
        int x;
        int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public Position pos;

    public PositionedRoom(RoomShape room, Position pos, int rotation)
    {
        this.room = room;
        this.pos = pos;
        this.rotation = rotation;
    }

}
