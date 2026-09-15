using System;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private const int MAX_MAP_SIZE_X = 64;
    private const int MAX_MAP_SIZE_Y = 64;

    private GenericHolder[,] _holders = new GenericHolder[MAX_MAP_SIZE_X, MAX_MAP_SIZE_Y];
    private Grid _mapGrid;

    private void Start()
    {
        _mapGrid = GetComponent<Grid>();
        ConveyorGeneration();
    }

    public void PlaceOnGrid(int x, int y)
    {
        
    }

    public void PlaceOnGrid(Vector2Int gridPosition)
    {
        
    }

    public void RemoveFromGrid(int x, int y)
    {
        
    }

    public void RemoveFromGrid(Vector2Int gridPosition)
    {
        
    }

    public GenericHolder GetItemFromGrid(int x, int y)
    {
        
    }

    public GenericHolder GetItemFromGrid(Vector2Int gridPosition)
    {
        
    }

    private void ConveyorGeneration()
    {
        
    }
}