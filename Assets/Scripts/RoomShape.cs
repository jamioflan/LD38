using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomShape
{

    public static readonly int maxMatrixWidth = 4;
    public static readonly int maxMatrixHeight = 4;

    public RoomBlock[,] matrix = new RoomBlock[maxMatrixWidth, maxMatrixHeight];

    public class Bounds
    {
        public int minX;
        public int minY;
        public int maxX;
        public int maxY;
    }
    public Bounds bounds;

    public DungeonPiece dungeonPiece;

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

    public RoomBlock addBlock(int x, int y)
    {
        Debug.Assert(this.matrix[x, y] == null, "Cannot add a room block to a room shape at an already occupied position!");
        RoomBlock roomBlock = new RoomBlock(this, x, y);
        this.matrix[x, y] = roomBlock;
        return roomBlock;
    }

    public int getNumBlocks()
    {
        int num = 0;
        foreach (RoomBlock block in this.matrix)
        {
            if (block != null)
            {
                num++;
            }
        }
        return num;
    }

    public void calculateBounds()
    {
        if (this.bounds != null)
        {
            return;
        }
        this.bounds.minX = maxMatrixWidth - 1;
        this.bounds.minY = maxMatrixWidth - 1;
        this.bounds.maxX = 0;
        this.bounds.maxY = 0;
        for (int x = 0; x < maxMatrixWidth; x++)
        {
            for (int y = 0; x < maxMatrixHeight; x++)
            {
                if (matrix[x, y] != null)
                {
                    if (x < this.bounds.minX) this.bounds.minX = x;
                    if (y < this.bounds.minY) this.bounds.minY = y;
                    if (x > this.bounds.maxX) this.bounds.maxX = x;
                    if (y > this.bounds.maxY) this.bounds.maxY = y;
                }
            }
        }
    }

    public int getWidth()
    {
        this.calculateBounds();
        return this.bounds.maxX - this.bounds.minX + 1;
    }

    public int getHeight()
    {
        this.calculateBounds();
        return this.bounds.maxY - this.bounds.minY + 1;
    }

    public float getComplexity()
    {
        float width = this.getWidth();
        float height = this.getHeight();
        float numBlocks = this.getNumBlocks();
        return Mathf.Max(width, height) * width * height / numBlocks;
    }

    //public RoomDoor[] getDoors()
    //{
    //    RoomDoor[] doorList = new RoomDoor[];
    //}


}
