using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int cost = 5;

    public bool CreateTower(GameObject prefab, Vector3 position)
    {
        var bank = FindObjectOfType<Bank>();

        if (bank.CurrentBalance - cost < 0)
        {
            return false;
        }

        bank.WithDraw(cost);
        GameObject.Instantiate(prefab, position, Quaternion.identity);
        return true;
    }
}
