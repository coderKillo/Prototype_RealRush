using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] private int startBalance = 150;

    private int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    static private Bank instance;
    static public Bank Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        currentBalance = startBalance;
        UpdateDisplay();
    }


    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateDisplay();
    }

    public void WithDraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateDisplay();

        if (currentBalance < 0)
        {
            ReloadScene();
        }
    }

    private void UpdateDisplay()
    {
        PlayerUI.Instance.ShowBalance(currentBalance);
    }

    private void ReloadScene()
    {
        var scene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.LoadScene(scene.buildIndex);
    }
}
