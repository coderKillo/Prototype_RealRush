using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Tile[] path;
    [SerializeField][Range(0, 5)] private float speed;

    public bool isMoving { get; private set; } = false;

    private Enemy enemy;

    private void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void FindPath()
    {
        path = GameObject.FindGameObjectWithTag("Path").GetComponentsInChildren<Tile>();
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

        FinishPath();
    }

    private void FinishPath()
    {
        isMoving = false;
        gameObject.SetActive(false);
        enemy.PenaltyGold();
    }
}
