using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room2
{
    public bool[,] cells = new bool[3,3];
    public int colour = 0;
    public bool[,,] doors = new bool[3, 3, 4];
}
