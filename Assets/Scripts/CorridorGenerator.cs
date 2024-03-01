using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorGenerator : RandomWalkGenerator
{
    [SerializeField] private int corridorLenght = 10;
    [SerializeField] private int corridorCount = 5;
    [SerializeField] [Range(0.1f,1f)] private float roomPercent = 0.6f;

   
    protected override void RunGeneration()
    {
        CorridorGeneration();
    }

    private void CorridorGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        //adds rooms to floor positions
        floorPositions.UnionWith(roomPositions);


        visualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, visualizer);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomPositions)
    {
        foreach (var end in deadEnds) 
        {
            if(roomPositions.Contains(end) == false)
            {
                var room = RunRandomWalk(iterations, walkLength, randomStartPerIteration, end);
                roomPositions.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var pos in floorPositions)
        {
            int neigbourCount = 0;
            foreach (var direction in Direction2D.directionsList)
            {
                if(floorPositions.Contains(pos + direction))
                    neigbourCount ++;
            }
            if(neigbourCount == 1)
            {
                deadEnds.Add(pos);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>(); 
        int roomsToCreateAmount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomsToCreateAmount).ToList();

        foreach (var roomPos in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(iterations, walkLength, randomStartPerIteration, roomPos);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions )
    {
        var currentPos = startPos;
        potentialRoomPositions.Add(currentPos);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = Algorithms.CorridorWalk(currentPos, corridorLenght);
            currentPos = corridor[corridor.Count-1];
            potentialRoomPositions.Add(currentPos);
            floorPositions.UnionWith(corridor);
        }
    }
}
