using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionedRoom
{

    public RoomShape room;
    public int Rotation; // Rotation: anticlockwise in steps of 90deg
    public struct Position
    {
        int x;
        int y;
    }
    public Position pos;
}
