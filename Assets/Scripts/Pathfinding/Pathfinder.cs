using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] private Vector2Int startCoordinates;
    [SerializeField] private Vector2Int destinationCoordinates;

    private Node currentSearchNode;
    private Node startNode;
    private Node destinationNode;

    private Queue<Node> frontier = new Queue<Node>();
    private Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    private Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    private GridManager gridManager;


    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    private void Start()
    {
        startNode = gridManager.GetNode(startCoordinates);
        destinationNode = gridManager.GetNode(destinationCoordinates);

        BreathFistSearch();
        BuildPath();
    }

    private void ExploreNeighbors()
    {
        var neighbors = new List<Node>();

        foreach (var direction in directions)
        {
            var currentCoord = currentSearchNode.coordinates;
            var neighborCoord = currentSearchNode.coordinates + direction;

            Node neighbor = gridManager.GetNode(neighborCoord);
            Node current = gridManager.GetNode(currentCoord);

            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        foreach (var neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    private void BreathFistSearch()
    {
        bool isRunning = true;

        frontier.Enqueue(startNode);
        reached.Add(startCoordinates, startNode);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;

            ExploreNeighbors();

            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        var path = new List<Node>();
        var currentNode = destinationNode;

        while (currentNode != null)
        {
            currentNode.isPath = true;
            path.Add(currentNode);

            currentNode = currentNode.connectedTo;
        }

        path.Reverse();

        return path;
    }
}
