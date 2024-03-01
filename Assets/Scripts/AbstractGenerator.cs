using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    [SerializeField] protected TilemapVisualizer visualizer = null;
    [SerializeField] protected Vector2Int startPos = Vector2Int.zero;

    public void GenerateDungeon()
    {
        visualizer.Clear();
        RunGeneration();
    }

    protected abstract void RunGeneration();
}
