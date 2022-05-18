using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitpoints = 5;
    [SerializeField] private int difficultyRamp = 2;
    [SerializeField] private Slider slider;

    private Enemy enemy;
    private int m_currentHitpoints = 0;

    void OnEnable()
    {
        m_currentHitpoints = maxHitpoints;
        slider.maxValue = maxHitpoints;
        slider.value = maxHitpoints;
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        m_currentHitpoints--;

        slider.value = m_currentHitpoints;

        if (m_currentHitpoints <= 0)
        {
            gameObject.SetActive(false);
            enemy.RewardGold();
            maxHitpoints += difficultyRamp;
        }
    }
}

