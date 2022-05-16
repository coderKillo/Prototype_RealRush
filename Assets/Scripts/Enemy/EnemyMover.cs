using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Waypoint> path;
    [SerializeField][Range(0, 5)] private float speed;

    public bool isMoving { get; private set; } = false;

    void Start()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    private void FindPath()
    {
        path.Clear();

        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Path");

        foreach (var waypoint in waypoints)
        {
            path.Add(waypoint.GetComponent<Waypoint>());
        }

        path.Sort((w1, w2) => Vector3.Distance(w1.transform.position, transform.position).CompareTo(Vector3.Distance(w2.transform.position, transform.position)));
    }

    private void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    private void PrintWaypoint()
    {
        foreach (var waypoint in path)
        {
            Debug.Log(waypoint.name);
        }
    }

    IEnumerator FollowPath()
    {
        foreach (var waypoint in path)
        {
            isMoving = true;

            var startPos = transform.position;
            var endPos = waypoint.transform.position;
            var travelPercent = 0f;

            transform.LookAt(endPos);

            while (travelPercent < 1f)
            {
                travelPercent += speed * Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        isMoving = false;
        Destroy(gameObject);
    }
}
