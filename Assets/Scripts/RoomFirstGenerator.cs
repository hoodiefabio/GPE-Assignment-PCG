using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstGenerator : RandomWalkGenerator
{
    [SerializeField] private int minRoomWidth = 4;
    [SerializeField] private int minRoomHeight = 4;

    [SerializeField] private int dungeonWidth = 100;
    [SerializeField] private int dungeonHeight = 100;
    [SerializeField] [Range(0,8)] private int roomOffset = 2;
    [SerializeField] private bool randomWalkRooms = false;

    // Pcg data
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionairy = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    private HashSet<Vector2Int> floorPositions;
    private HashSet<Vector2Int> corridorPositions;

    protected override void RunGeneration()
    {
        CreateRooms();
        FillRooms();
    }

    private void FillRooms()
    {
        var playerRoom = roomsDictionairy.First();
        Vector2Int fartherst = Vector2Int.zero;
        float lenght = float.MinValue;
        foreach (var roomPoint in roomsDictionairy)
        {
            float currentDistance = Vector2.Distance(playerRoom.Key, roomPoint.Key);
            if (currentDistance < lenght)
            {
                lenght = currentDistance;
                fartherst = roomPoint.Key;
            }
        }
        visualizer.PaintSingleBoss(fartherst);
        Debug.Log(fartherst.ToString());
        visualizer.PaintSinglePlayer(playerRoom.Key);
    }

    private void CreateRooms()
    {
        var roomList = Algorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int) startPos, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
            floor = CreateRoomsWalker(roomList);
        else
            floor = CreateSimpleRooms(roomList);

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);

        floor.UnionWith(corridors);

        visualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, visualizer);
    }

    private HashSet<Vector2Int> CreateRoomsWalker(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomList.Count; i++)
        {
            var roomBound = roomList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBound.center.x), Mathf.RoundToInt(roomBound.center.y));
            var roomFloor = RunRandomWalk(iterations,walkLength,randomStartPerIteration, roomCenter);

            foreach(var pos in roomFloor)
            {
                if(pos.x >= (roomBound.xMin + roomOffset) && pos.x <= (roomBound.xMax - roomOffset) && pos.y >= (roomBound.yMin + roomOffset) && pos.y <= (roomBound.yMax - roomOffset))
                {
                    floor.Add(pos);
                    SaveRoomData(roomCenter, roomFloor);
                }
            }
        }
        return floor;
    }

    private void SaveRoomData(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor)
    {
        roomsDictionairy[roomCenter] = roomFloor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosest(currentCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentCenter, closest);
            currentCenter = closest;
            corridors.UnionWith(newCorridor);
            corridorPositions = new HashSet<Vector2Int>(corridors);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentCenter, Vector2Int closest)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var pos = currentCenter;
        corridor.Add(pos);
        while (pos.y != closest.y)
        {
            if(closest.y > pos.y)
            {
                pos += Vector2Int.up;
            }
            else if (closest.y < pos.y)
            {
                pos += Vector2Int.down;
            }
            corridor.Add(pos);
        }
        while (pos.x != closest.x)
        {
            if (closest.x > pos.x)
            {
                pos += Vector2Int.right;
            }
            else if (closest.x < pos.x)
            {
                pos += Vector2Int.left;
            }
            corridor.Add(pos);
        }
        return corridor;
    }

    private Vector2Int FindClosest(Vector2Int currentCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float lenght = float.MaxValue;
        foreach (var room in roomCenters)
        {
            float currentDistance = Vector2.Distance(currentCenter, room);
            if (currentDistance < lenght)
            {
                lenght = currentDistance;
                closest = room;
            }
        }
        return closest;
    }



    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = roomOffset; col < room.size.x - roomOffset; col++)
            {
                for (int row = roomOffset; row < room.size.y - roomOffset; row++)
                {
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(pos);
                }
            }
        }
        return floor;
    }
}
