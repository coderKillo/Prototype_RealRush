using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balance;

    static private PlayerUI instance;
    static public PlayerUI Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ShowBalance(int value)
    {
        balance.text = $"Gold: {value}";
    }
}
