using System.Collections;
using System.Collections.Generic;
using Atsar;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridNode))]
public class AstarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // base.DrawDefaultInspector();
        // if (GUILayout.Button("Generate Grid"))
        // {
        //     ((GridNode)target).GenerateGridNode();
        // }

        // if (GUILayout.Button("Clear Grid"))
        // {
        //     ((GridNode)target).CleanGridNode();
        // }
    }
}
