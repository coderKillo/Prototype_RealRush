using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;

    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    private Vector2Int coordinates;

    private void Start()
    {
        coordinates = GridManager.GetCoordinatesFromPosition(transform.position);

        if (!isPlaceable)
        {
            GridManager.Instance.BlockNode(coordinates);
        }
    }

    private void OnMouseDown()
    {
        if (!GridManager.Instance.ContainsKey(coordinates)) { return; }
        if (!GridManager.Instance.GetNode(coordinates).isWalkable) { return; }
        if (Pathfinder.Instance.WillBlockPath(coordinates)) { return; }

        if (towerPrefab.CreateTower(towerPrefab.gameObject, transform.position))
        {
            GridManager.Instance.BlockNode(coordinates);
            Pathfinder.Instance.NotifyReceivers();
        }
    }
}
