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
    [SerializeField] private Color notGridColor = Color.black;
    [SerializeField] private Color exploredColor = Color.yellow;
    [SerializeField] private Color pathColor = new Color(1f, 0.5f, 0f);

    private TextMeshPro label;
    private Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

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
        if (GridManager.Instance == null) { return; }

        Node node = GridManager.Instance.GetNode(coordinates);

        if (node == null)
        {
            label.color = notGridColor;
            return;
        }

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
        else
        {
            label.color = defaultColor;
        }
    }

    private void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    private void DisplayCoordinates()
    {
        if (GridManager.Instance == null) { return; }

        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / GridManager.Instance.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / GridManager.Instance.UnityGridSize);
        label.text = $"{coordinates.x}, {coordinates.y}";
    }
}
