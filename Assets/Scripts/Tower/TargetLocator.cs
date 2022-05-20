using System.Runtime.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    [SerializeField] private ParticleSystem projectileParticals;
    [SerializeField] private float range = 15f;

    private Transform target;

    void Update()
    {
        FindClosedTarget();
        AimAtTarget();
    }

    private void FindClosedTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float maxDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < maxDistance)
            {
                maxDistance = distance;
                target = enemy.transform;
            }
        }

    }

    private void AimAtTarget()
    {
        if (target == null) { return; }

        float targetDistance = Vector3.Distance(target.transform.position, transform.position);

        Attack(targetDistance < range);

        if (target)
        {
            weapon.LookAt(target.position);
        }
    }

    private void Attack(bool active)
    {
        var emission = projectileParticals.emission;
        emission.enabled = active;
    }
}
