using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkGenerator : AbstractGenerator
{
    //paramiters
    [SerializeField] protected int iterations = 10;
    [SerializeField] protected int walkLength = 5;
    [SerializeField] protected bool randomStartPerIteration = true;


    protected override void RunGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(iterations,walkLength,randomStartPerIteration,startPos);
        visualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, visualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(int iterations, int walkLength, bool randomStartPerIteration, Vector2Int position)
    {
        var currentPos = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < iterations; i++)
        {
            var path = Algorithms.RandomWalk(currentPos, walkLength);
            floorPositions.UnionWith(path);
            if (randomStartPerIteration)
            {
                currentPos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }

    
}
