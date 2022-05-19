using System.Threading;
using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoodinateLabler : MonoBehaviour
{
    [Header("Color Settings")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color blockedColor = Color.gray;
    [SerializeField] private Color exploredColor = Color.yellow;
    [SerializeField] private Color pathColor = new Color(1f, 0.5f, 0f);

    private TextMeshPro label;
    private Vector2Int coordinates = new Vector2Int();
    private GridManager gridManager;

    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        gridManager = FindObjectOfType<GridManager>();

        DisplayCoordinates();
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            label.enabled = true;
        }

        ColorCoordinates();
        ToggleLabels();
    }

    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            label.enabled = label.enabled ? false : true;
        }
    }

    private void ColorCoordinates()
    {
        if (gridManager == null) { return; }

        Node node = gridManager.GetNode(coordinates);

        if (node == null) { return; }

        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if (node.isPath)
        {
            label.color = pathColor;
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
    }

    private void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    private void DisplayCoordinates()
    {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / 10);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / 10);
        label.text = $"{coordinates.x}, {coordinates.y}";
    }
}
