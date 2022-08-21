using UnityEngine;
using UnityEditor;

namespace MapDesigner
{
    [CustomEditor(typeof(PFTTileSlot))]
    [CanEditMultipleObjects]
    public class PFTTileSlotEditor : Editor
    {
        PFTTileSlot tileslot;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            tileslot = target as PFTTileSlot;
            if (GUILayout.Button("Generate Tile"))
                tileslot.GenerateTile();

            if (GUILayout.Button("Delete Tile"))
                tileslot.DeleteTile();
        }
    }
}