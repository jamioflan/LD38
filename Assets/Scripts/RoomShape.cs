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

    private int numBlocks = -1;
    private int complexity = -1;

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
        if (this.numBlocks > -1)
        {
            return this.numBlocks;
        }
        this.numBlocks = 0;
        foreach (RoomBlock block in this.matrix)
        {
            if (block != null)
            {
                this.numBlocks++;
            }
        }
        return this.numBlocks;
    }

    public void calculateBounds()
    {
        if (this.bounds != null)
        {
            return;
        }
        this.bounds = new Bounds();
        this.bounds.minX = maxMatrixWidth - 1;
        this.bounds.minY = maxMatrixWidth - 1;
        this.bounds.maxX = 0;
        this.bounds.maxY = 0;
        for (int x = 0; x < maxMatrixWidth; x++)
        {
            for (int y = 0; y < maxMatrixHeight; y++)
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

    public int getComplexity()
    {
        if (this.complexity <= -1)
        {
            float width = this.getWidth();
            float height = this.getHeight();
            float numBlocks = this.getNumBlocks();
            float cf = (Mathf.Max(width, height) - 1) * width * height / numBlocks * 100;
            this.complexity = Mathf.FloorToInt(cf);
        }
        return complexity;
    }

    public RoomBlock getBlock(Position position)
    {
        if (position.x < 0 || position.y < 0 || position.x >= RoomShape.maxMatrixWidth || position.y >= RoomShape.maxMatrixHeight)
        {
            return null;
        }
        else
        {
            return this.matrix[position.x, position.y];
        }
    }

    public void generateWalls()
    {
        for (int x = 0; x < RoomShape.maxMatrixWidth; x++)
        {
            for (int y = 0; y < RoomShape.maxMatrixHeight; y++)
            {
                if (this.matrix[x, y] != null)
                {
                    if (x == 0 || this.matrix[x - 1, y] == null) this.matrix[x, y].walls[1] = new RoomWall(this.matrix[x, y]);
                    if (y == 0 || this.matrix[x, y - 1] == null) this.matrix[x, y].walls[2] = new RoomWall(this.matrix[x, y]);
                    if (x == RoomShape.maxMatrixWidth - 1 || this.matrix[x + 1, y] == null) this.matrix[x, y].walls[3] = new RoomWall(this.matrix[x, y]);
                    if (y == RoomShape.maxMatrixHeight - 1 || this.matrix[x, y + 1] == null) this.matrix[x, y].walls[0] = new RoomWall(this.matrix[x, y]);
                }
            }
        }
    }

    public RoomWall[] getWalls()
    {
        RoomWall[] walls = new RoomWall[RoomShape.maxMatrixWidth* RoomShape.maxMatrixHeight*4];
        int indexAt = 0;
        foreach (RoomBlock block in this.matrix)
        {
            if (block != null)
            {
                for (int w = 0; w < 4; w++)
                {
                    if (block.walls[w] != null) walls[indexAt++] = block.walls[w];
                }
            }
        }
        return walls;
    }

        //public RoomDoor[] getDoors()
        //{
        //    RoomDoor[] doorList = new RoomDoor[];
        //}


}


public class GoldRoomShape : RoomShape
{

    public static int WIDTH = 3;
    public static int HEIGHT = 3;

    public GoldRoomShape()
    {
        for (int x = 0; x<WIDTH; x++)
        {
            for (int y=0; y<HEIGHT; y++)
            {
                this.matrix[x, y] = new RoomBlock(this,x,y);
            }
        }
    }

}