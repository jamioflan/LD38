using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBlock {
    
    public class Walls
    {
        public bool north;
        public bool west;
        public bool south;
        public bool east;

        public Walls()
        {
            north = false;
            east = false;
            south = false;
            west = false;
        }
    }

    public RoomShape parentRoom = null;


}
