using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[ExecuteAlways]
public class CoodinateLabler : MonoBehaviour
{
    private TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        DisplayCoordinates();
        UpdateObjectName();
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            return;
        }

        DisplayCoordinates();
        UpdateObjectName();
    }

    private void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    private void DisplayCoordinates()
    {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);
        label.text = $"{coordinates.x}, {coordinates.y}";
    }
}
