using UnityEngine;

namespace MapDesigner
{
    [CreateAssetMenu(fileName = "tileholder", menuName = "ScriptableObjects/Tile Holder", order = 0)]
    public class SO_TilePrefabHolder : ScriptableObject
    {
        [SerializeField]
        TileTerrainType _terrainType;
        public TileTerrainType TerrainType => _terrainType;

        [SerializeField]
        GameObject _tilePrefab;
        public GameObject TilePrefab => _tilePrefab;
    }
}