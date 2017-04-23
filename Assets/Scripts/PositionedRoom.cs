using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PositionedRoomSkel
{

    public Grid.Position pos;
    public int rotation; // Rotation: anticlockwise in steps of 90deg

    public PositionedRoomSkel(Grid.Position pos, int rotation)
    {
        this.pos = pos;
        this.rotation = rotation;
    }

}

[System.Serializable]
public class PositionedRoom
{

    public RoomShape room;
    public int rotation; // Rotation: anticlockwise in steps of 90deg

    public Grid.Position pos;
    
    // Position relative to pos
    public class Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
    
    public class Bounds
    {
        public int minX;
        public int minY;
        public int maxX;
        public int maxY;
    }
    public Bounds bounds = new Bounds();

    public PositionedRoom(RoomShape room)
    {
        this.room = room;
    }

    public void createFromSkel(PositionedRoomSkel skel)
    {
        this.pos = skel.pos;
        this.rotation = skel.rotation;
    }

    // Rotate an internal position
    public PositionedRoom.Position toRelativePosition(RoomShape.Position rspos)
    {
        Position rrpos = new Position(0, 0);
        if (this.rotation == 0) { rrpos.x = rspos.x; rrpos.y = rspos.y; }
        if (this.rotation == 1) { rrpos.x = 0 - rspos.y; rrpos.y = rspos.x; }
        if (this.rotation == 2) { rrpos.x = 0 - rspos.x; rrpos.y = 0 - rspos.y; }
        if (this.rotation == 3) { rrpos.x = rspos.y; rrpos.y = 0 - rspos.x; }
        return rrpos;
    }

    public PositionedRoom.Position toRelativePosition(Grid.Position gpos)
    {
        return new PositionedRoom.Position(gpos.x - this.pos.x, gpos.y - this.pos.y);
    }

    // Unrotate a relative position (so that it can be used inside RoomShape.matix)
    public RoomShape.Position toInternalPosition(PositionedRoom.Position rpos)
    {
        RoomShape.Position rspos = new RoomShape.Position(0, 0);
        if (this.rotation == 0) { rspos.x = rpos.x; rspos.y = rpos.y; }
        if (this.rotation == 1) { rspos.x = rpos.y; rspos.y = 0 - rpos.x; }
        if (this.rotation == 2) { rspos.x = 0 - rpos.x; rspos.y = 0 - rpos.y; }
        if (this.rotation == 3) { rspos.x = 0 - rpos.y; rspos.y = rpos.x; }
        return rspos;
    }


    public RoomShape.Position toInternalPosition(Grid.Position gpos)
    {
        return toInternalPosition(toRelativePosition(gpos));
    }

    public Grid.Position toGridPosition(PositionedRoom.Position rpos)
    {
        return new Grid.Position(rpos.x + this.pos.x, rpos.y + this.pos.y);
    }

    public Grid.Position toGridPosition(RoomShape.Position rspos)
    {
        PositionedRoom.Position rpos = toRelativePosition(rspos);
        return new Grid.Position(rpos.x + this.pos.x, rpos.y + this.pos.y);
    }

    public RoomBlock getRoomBlock(RoomShape.Position position)
    {
        return this.room.getBlock(position);
    }


    public RoomBlock getRoomBlock(PositionedRoom.Position position)
    {
        RoomShape.Position internalPosition = this.toInternalPosition(position);
        return this.getRoomBlock(internalPosition);
    }


    public RoomBlock getRoomBlock(Grid.Position position)
    {
        RoomShape.Position internalPosition = this.toInternalPosition(position);
        return this.getRoomBlock(internalPosition);
    }


    public void calculateBounds()
    {
        if (this.rotation % 4 == 0)
        {
            this.bounds.minX = this.room.bounds.minX;
            this.bounds.minY = this.room.bounds.minY;
            this.bounds.maxX = this.room.bounds.maxX;
            this.bounds.maxY = this.room.bounds.maxY;
        }
        if (this.rotation % 4 == 1)
        {
            this.bounds.minX = 0-this.room.bounds.maxY;
            this.bounds.minY = this.room.bounds.minX;
            this.bounds.maxX = 0-this.room.bounds.maxY;
            this.bounds.maxY = this.room.bounds.minY;
        }
        if (this.rotation % 4 == 2)
        {
            this.bounds.minX = 0-this.room.bounds.maxX;
            this.bounds.minY = 0-this.room.bounds.maxY;
            this.bounds.maxX = 0-this.room.bounds.minX;
            this.bounds.maxY = 0-this.room.bounds.minY;
        }
        if (this.rotation % 4 == 3)
        {
            this.bounds.minX = this.room.bounds.minY;
            this.bounds.minY = 0-this.room.bounds.maxX;
            this.bounds.maxX = this.room.bounds.maxY;
            this.bounds.maxY = 0-this.room.bounds.minX;
        }
    }

    public bool collides(PositionedRoom otherPosRoom)
    {
        if (otherPosRoom == null || this.pos == null || otherPosRoom.pos == null) return false;
        for (int x = 0; x < RoomShape.maxMatrixWidth; x++)
        {
            for (int y = 0; y < RoomShape.maxMatrixHeight; y++)
            {
                RoomShape.Position internalPosition = new RoomShape.Position(x, y);
                Grid.Position gridPosition = toGridPosition(internalPosition);
                if (this.getRoomBlock(internalPosition) != null && otherPosRoom.getRoomBlock(gridPosition) != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool collides(PositionedRoom[] otherPosRooms)
    {
        foreach (PositionedRoom otherPosRoom in otherPosRooms)
        {
            if (otherPosRoom != this && this.collides(otherPosRoom)) return true;
        }
        return false;
    }

}
