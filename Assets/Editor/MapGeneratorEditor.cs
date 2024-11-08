using System.Collections;
using System.Collections.Generic;
using TileMap;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if (GUILayout.Button("GenerateMap"))
        {
            ((MapGenerator)target).GenerateMap();
        }
        if (GUILayout.Button("CleanTileMap"))
        {
            ((MapGenerator)target).CleanTileMap();
        }
    }
}
