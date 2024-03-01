using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer visualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.directionsList);
        foreach (var position in basicWallPositions) 
        {
            visualizer.PaintSingleWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach(var direction in directions)
            {
                var neighBourPosition = position + direction;
                if (floorPositions.Contains(neighBourPosition) == false)
                {
                    wallPositions.Add(neighBourPosition);
                }
            }
        }
        return wallPositions;
    }
}
