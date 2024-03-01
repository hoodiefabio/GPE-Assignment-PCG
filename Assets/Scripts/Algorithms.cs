using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Algorithms
{
    //Random walk algoritm
   public static HashSet<Vector2Int> RandomWalk(Vector2Int startPos, int walkLenght)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPos);
        var prevPos = startPos;

        for(int i = 0; i < walkLenght; i++)
        {
            var newPos = prevPos + Direction2D.GetRandomDirection();
            path.Add(newPos);
            prevPos = newPos;
        }
        return path;
    }

    public static List<Vector2Int> CorridorWalk(Vector2Int startPos, int corridorLenght)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomDirection();
        var currentPos = startPos;
        corridor.Add(currentPos);

        for (int i = 0;i < corridorLenght;i++)
        {
            currentPos += direction;
            corridor.Add(currentPos);
        }
        return corridor;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQ = new Queue<BoundsInt>();
        List<BoundsInt> roomList = new List<BoundsInt>();
        roomsQ.Enqueue(spaceToSplit);
        while (roomsQ.Count > 0)
        {
            var room = roomsQ.Dequeue();
            if(room.size.y >= minHeight && room.size.x >= minWidth) 
            {
                if (Random.value < 0.5f)
                {
                    if(room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQ, room);
                    }
                    else if (room.size.y >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQ, room);
                    }
                    else
                    {
                        roomList.Add(room);
                    }
                }
                else
                {
                    
                    if (room.size.y >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQ, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQ, room);
                    }
                    else
                    {
                        roomList.Add(room);
                    }
                }
            }
        }
        return roomList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQ, BoundsInt room)
    {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit,room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x-xSplit, room.size.y, room.size.z));
        roomsQ.Enqueue(room1);
        roomsQ.Enqueue(room2);
    }

    private static void SplitHorizontally( int minHeight, Queue<BoundsInt> roomsQ, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQ.Enqueue(room1);
        roomsQ.Enqueue(room2);
    }
}



public static class Direction2D
{
    public static List<Vector2Int> directionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1),//up
        new Vector2Int(0, -1),//down
        new Vector2Int(-1, 0),//left
        new Vector2Int(1, 0)//right
    };

    public static Vector2Int GetRandomDirection()
    {
        return directionsList[Random.Range(0, directionsList.Count)];
    }
}
