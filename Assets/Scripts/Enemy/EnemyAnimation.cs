using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private EnemyMover m_mover;

    void Start()
    {
        m_mover = GetComponent<EnemyMover>();
    }

    void Update()
    {
        if (m_mover.isMoving)
        {
            animator.Play("Run");
        }
        else
        {
            animator.Play("Idle");
        }
    }
}
