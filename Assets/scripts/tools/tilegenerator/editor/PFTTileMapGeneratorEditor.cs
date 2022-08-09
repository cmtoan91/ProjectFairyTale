using UnityEngine;
using UnityEditor;
using System;

namespace MapDesigner
{
    [CustomEditor(typeof(PFTTileMapGenerator))]
    public class PFTTileMapGeneratorEditor : Editor
    {
        int _tileCountX;

        int _tileCountZ;

        float _tileSize;

        float _offset;
        Material _matWhite;
        PFTTileMapGenerator _generator;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _generator = target as PFTTileMapGenerator;
            _tileCountX =_generator.TileCountX;
            _tileCountZ = _generator.TileCountZ;
            _tileSize = _generator.TileSize;
            _offset = _generator.OffSet;

            if (GUILayout.Button("Generate Tile Map"))
                _generator.GenerateAllTiles(_tileCountX, _tileCountZ, _tileSize, _offset);
            
            if (GUILayout.Button("Clear Tile Map"))
                _generator.ClearAllTile();

            GUILayout.Label("Data");

            if(_generator.TilePrefabData.Count > 0)
            {
                TileTerrainType[] allterrains = Enum.GetValues(typeof(TileTerrainType)) as TileTerrainType[];
                foreach (TileTerrainType type in allterrains)
                {
                    GameObject prefab;
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.TextField(type.ToString());
                    if (_generator.TilePrefabData.TryGetValue(type, out prefab))
                    {
                        EditorGUILayout.TextField(prefab.name);
                    }
                    else
                    {
                        EditorGUILayout.TextField("not found");
                    }
                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.TextField("No Data found or Data not loaded. Please load data");
            }

            if (GUILayout.Button("Load Tile Data"))
                _generator.RefreshData();

            if (GUILayout.Button("Save Tile Map"))
                _generator.SaveTileMap();
        }
    }
}