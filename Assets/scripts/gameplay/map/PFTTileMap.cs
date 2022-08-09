using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MapDesigner
{
    public class PFTTileMap : MonoBehaviour
    {
        #region props
        [SerializeField]
        List<PFTTileSlot> _allSlot = new List<PFTTileSlot>();
        public List<PFTTileSlot> AllSlot => _allSlot;

        Dictionary<TileTerrainType, GameObject> _tilePrefabData = new Dictionary<TileTerrainType, GameObject>();
        #endregion

        public void AddTileSlotToMap(PFTTileSlot slot)
        {
            _allSlot.Add(slot);
        }

        public void ClearTileMap()
        {
            _allSlot.Clear();
        }

        public void LoadTileMapData(Dictionary<TileTerrainType, GameObject> tileData)
        {
            _tilePrefabData = tileData;
        }

        public GameObject GetTilePrefab(TileTerrainType terrainType)
        {
            if (_tilePrefabData.ContainsKey(terrainType))
                return _tilePrefabData[terrainType];
            else
                return null;
        }

        private void OnValidate()
        {
            _allSlot = GetComponentsInChildren<PFTTileSlot>().ToList();
        }
    }
}