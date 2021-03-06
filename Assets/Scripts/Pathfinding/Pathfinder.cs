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
    private Dictionary<Vector2Int, Node> Grid = new Dictionary<Vector2Int, Node>();

    static private Pathfinder instance;
    static public Pathfinder Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        startNode = GridManager.Instance.GetNode(startCoordinates);
        destinationNode = GridManager.Instance.GetNode(destinationCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        GridManager.Instance.ResetNotes();
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
            if (GridManager.Instance.ContainsKey(neighborCoord))
            {
                neighbors.Add(GridManager.Instance.GetNode(neighborCoord));
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

        frontier.Enqueue(GridManager.Instance.GetNode(coordinates));
        reached.Add(coordinates, GridManager.Instance.GetNode(coordinates));

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
        if (GridManager.Instance.ContainsKey(coordinates))
        {
            bool previousState = GridManager.Instance.GetNode(coordinates).isWalkable;

            GridManager.Instance.GetNode(coordinates).isWalkable = false;
            List<Node> newPath = GetNewPath();
            GridManager.Instance.GetNode(coordinates).isWalkable = previousState;

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
