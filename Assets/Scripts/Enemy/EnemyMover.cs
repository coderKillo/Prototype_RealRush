using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Waypoint[] path;
    [SerializeField][Range(0, 5)] private float speed;

    public bool isMoving { get; private set; } = false;

    private void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    private void FindPath()
    {
        path = GameObject.FindGameObjectWithTag("Path").GetComponentsInChildren<Waypoint>();
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
        gameObject.SetActive(false);
    }
}
