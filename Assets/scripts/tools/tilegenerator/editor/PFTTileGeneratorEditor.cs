using UnityEngine;
using UnityEditor;

namespace MapDesigner
{
    [CustomEditor(typeof(PFTTileMapGenerator))]
    public class PFTTileGeneratorEditor : Editor
    {
        int _tileCountX;

        int _tileCountZ;

        float _tileSize;

        float _offset;
        Material _matWhite;
        PFTTileMapGenerator _generator;
        public override void OnInspectorGUI()
        {
            _generator = target as PFTTileMapGenerator;
                        
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_tileCountX"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_tileCountZ"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_tileSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_offset"));

            if (EditorGUILayout.PropertyField(serializedObject.FindProperty("_matWhite")))
            {
                EditorGUIUtility.ShowObjectPicker<Material>(_generator.MaterialWhite, true, "", 0);
            }

            serializedObject.ApplyModifiedProperties();

            _tileCountX =_generator.TileCountX;
            _tileCountZ = _generator.TileCountZ;
            _tileSize = _generator.TileSize;
            _offset = _generator.OffSet;

            if (GUILayout.Button("Generate Tile Map"))
            {
                _generator.GenerateAllTiles(_tileCountX, _tileCountZ, _tileSize, _offset);
            }

            if (GUILayout.Button("Clear Tile Map"))
            {
                _generator.ClearAllTile();
            }
        }

        private void OnSceneGUI()
        {
            
        }
    }
}