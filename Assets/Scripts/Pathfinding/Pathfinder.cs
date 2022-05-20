using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] private Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] private Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    private Node currentSearchNode;
    private Node startNode;
    private Node destinationNode;

    private Queue<Node> frontier = new Queue<Node>();
    private Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    private Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    private GridManager gridManager;
    private Dictionary<Vector2Int, Node> Grid = new Dictionary<Vector2Int, Node>();


    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        startNode = gridManager.GetNode(startCoordinates);
        destinationNode = gridManager.GetNode(destinationCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNotes();
        BreathFistSearch(coordinates);
        return BuildPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    private void ExploreNeighbors()
    {
        var neighbors = new List<Node>();

        foreach (var direction in directions)
        {
            var neighborCoord = currentSearchNode.coordinates + direction;
            if (gridManager.ContainsKey(neighborCoord))
            {
                neighbors.Add(gridManager.GetNode(neighborCoord));
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

    private void BreathFistSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(gridManager.GetNode(coordinates));
        reached.Add(coordinates, gridManager.GetNode(coordinates));

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

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (gridManager.ContainsKey(coordinates))
        {
            bool previousState = gridManager.GetNode(coordinates).isWalkable;

            gridManager.GetNode(coordinates).isWalkable = false;
            List<Node> newPath = GetNewPath();
            gridManager.GetNode(coordinates).isWalkable = previousState;

            if (newPath.Count < 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", SendMessageOptions.DontRequireReceiver);
    }
}
