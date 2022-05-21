using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int cost = 5;
    [SerializeField] private float buildTime = 1f;

    public bool CreateTower(GameObject prefab, Vector3 position)
    {
        if (Bank.Instance.CurrentBalance - cost < 0)
        {
            return false;
        }

        Bank.Instance.WithDraw(cost);
        GameObject.Instantiate(prefab, position, Quaternion.identity);

        return true;
    }

    private void Start()
    {
        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in transform)
        {
            yield return new WaitForSeconds(buildTime / transform.childCount);
            child.gameObject.SetActive(true);
            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(true);
            }
        }
    }
}
