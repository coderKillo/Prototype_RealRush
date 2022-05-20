using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;

    [Tooltip("Should match the Unity Editor snap settings")]
    [SerializeField] private int unityGridSize = 10;
    public int UnityGridSize { get { return unityGridSize; } }

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    private void Awake()
    {
        CreateGrid();
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }

        return null;
    }

    public bool ContainsKey(Vector2Int coordinates)
    {
        return grid.ContainsKey(coordinates);
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        return new Vector2Int(
            Mathf.RoundToInt(position.x / unityGridSize),
            Mathf.RoundToInt(position.z / unityGridSize)
        );
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        return new Vector3(
            coordinates.x * unityGridSize,
            0,
            coordinates.y * unityGridSize
        );
    }

    public void ResetNotes()
    {
        foreach (KeyValuePair<Vector2Int, Node> item in grid)
        {
            item.Value.isExplored = false;
            item.Value.isPath = false;
            item.Value.connectedTo = null;
        }
    }

    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
}
