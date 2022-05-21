using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0, 5)] private float speed;

    List<Node> path = new List<Node>();

    public bool isMoving { get; private set; } = false;

    private Enemy enemy;

    private void OnEnable()
    {
        ReturnToStart();
        RecalculatePath();
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void RecalculatePath()
    {
        var coordinates = GridManager.GetCoordinatesFromPosition(transform.position);

        StopAllCoroutines();
        path.Clear();
        path = Pathfinder.Instance.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    private void ReturnToStart()
    {
        transform.position = GridManager.GetPositionFromCoordinates(Pathfinder.Instance.StartCoordinates);
    }

    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            isMoving = true;

            var startPos = transform.position;
            var endPos = GridManager.GetPositionFromCoordinates(path[i].coordinates);
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
