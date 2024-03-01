using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private Tilemap wallMap;
    [SerializeField] private Tilemap itemMap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTop;
    [SerializeField] private TileBase playerTile;
    [SerializeField] private TileBase bossTile;
    [SerializeField] private TileBase daggerTile;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        Clear();
        PaintTiles(floorPositions, floorMap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap fMap, TileBase fTile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(fMap, fTile, position);
        }
    }

    private void PaintSingleTile(Tilemap fMap, TileBase fTile, Vector2Int position)
    {
        var tilePos = fMap.WorldToCell((Vector3Int)position);
        fMap.SetTile(tilePos, fTile);
    }

    public void Clear()
    {
        floorMap.ClearAllTiles();
        wallMap.ClearAllTiles();
        itemMap.ClearAllTiles();
    }

    internal void PaintSingleWall(Vector2Int position)
    {
        PaintSingleTile(wallMap, wallTop, position);
    }

    internal void PaintSinglePlayer(Vector2Int position)
    {
        PaintSingleTile(itemMap, playerTile, position);
    }

    internal void PaintSingleBoss(Vector2Int position)
    {
        PaintSingleTile(itemMap, bossTile, position);
    }

    internal void PaintSingleDagger(Vector2Int position)
    {
        PaintSingleTile(itemMap, daggerTile, position);
    }
}
