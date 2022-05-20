using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;

    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    private GridManager gridManager;
    private Pathfinder pathfinder;
    private Vector2Int coordinates;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    private void OnMouseDown()
    {
        if (gridManager == null) { return; }
        if (pathfinder == null) { return; }
        if (!gridManager.ContainsKey(coordinates)) { return; }

        if (gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates))
        {
            bool isPlaced = towerPrefab.CreateTower(towerPrefab.gameObject, transform.position);
            isPlaceable = !isPlaced;

            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }
}
