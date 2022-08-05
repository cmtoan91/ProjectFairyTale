using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PFTTileGenerator))]
public class PFTTileGeneratorEditor : Editor
{
    int _tileCountX;

    int _tileCountZ;

    float _tileSize;

    float _offset;
    Material _matWhite;
    PFTTileGenerator _generator;
    public override void OnInspectorGUI()
    {
        _generator = target as PFTTileGenerator;

        _tileCountX = EditorGUILayout.IntField("Tile Count X", _generator.TileCountX);
        _tileCountZ = EditorGUILayout.IntField("Tile Count Z", _generator.TileCountZ);
        _tileSize = EditorGUILayout.FloatField("Tile Size", _generator.TileSize);
        _offset = EditorGUILayout.FloatField("Tile Offset", _generator.OffSet);
        serializedObject.Update();
        EditorGUILayout.LabelField("Game Data:");
        if (EditorGUILayout.PropertyField(serializedObject.FindProperty("_matWhite")))
        {
            EditorGUIUtility.ShowObjectPicker<Material>(_generator.MaterialWhite, true, "", 0);
        }

        if (GUILayout.Button("Generate Tile Map"))
        {
            _generator.GenerateAllTiles(_tileCountX, _tileCountZ, _tileSize, _offset);
        }

        if(GUILayout.Button("Clear Tile Map"))
        {
            _generator.ClearAllTile();
        }
    }

}
