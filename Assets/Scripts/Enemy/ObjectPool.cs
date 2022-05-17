using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private float spawntime = 1f;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private GameObject enemyPrefab;

    private GameObject[] pool;

    private void Awake()
    {
        initPool();
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void initPool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = GameObject.Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawntime);
        }
    }

    private void EnableObjectInPool()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return;
            }
        }
    }
}
