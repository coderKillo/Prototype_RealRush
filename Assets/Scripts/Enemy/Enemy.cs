using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int goldReward = 25;
    [SerializeField] private int goldPenalty = 25;

    public void RewardGold()
    {
        Bank.Instance.Deposit(goldReward);
    }

    public void PenaltyGold()
    {
        Bank.Instance.WithDraw(goldPenalty);
    }
}
