using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Waypoint> path;
    [SerializeField] private float stepTimeout = 1f;

    void Start()
    {
        StartCoroutine(move());
    }

    private void printWaypoint()
    {
        foreach (var waypoint in path)
        {
            Debug.Log(waypoint.name);
        }
    }

    IEnumerator move()
    {
        foreach (var waypoint in path)
        {
            yield return new WaitForSeconds(stepTimeout);
            transform.position = waypoint.transform.position;
        }
    }
}
