using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomShape
{

    public static readonly int maxMatrixWidth = 4;
    public static readonly int maxMatrixHeight = 4;

    public RoomBlock[,] matrix = new RoomBlock[maxMatrixWidth, maxMatrixHeight];

    //public RoomDoor[] getDoors()
    //{
    //    RoomDoor[] doorList = new RoomDoor[];
    //}


}
