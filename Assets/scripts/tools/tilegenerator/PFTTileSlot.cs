using MainGame;
using System.Collections.Generic;
using UnityEngine;

namespace MapDesigner {
    public class PFTTileSlot : MonoBehaviour
    {
        #region props
        [SerializeField]
        TileTerrainType _terrainType = TileTerrainType.Dirt;


        [SerializeField]
        float _tileHeight = 1;

        float _tileSize;
        float _prefabTileSize = 0.5f;
        float _prefabTileHeight = 1;

        Vector3[] _cornersOffSet;
        public Vector3[] CornerOffSet => _cornersOffSet;

        [SerializeField]
        Vector3[] _corners;
        public Vector3[] Corners => _corners;

        [SerializeField]
        Vector2Int _slotCoordinate;
        public Vector2Int SlotCoordinate => _slotCoordinate;

        [SerializeField]
        PFTTileMap _map;
        #endregion
        public void SetMap(PFTTileMap generator)
        {
            _map = generator;
        }

        public void SetSize(float size, float prefabSize, float prefabHeight)
        {
            _tileSize = size;
            _prefabTileSize = prefabSize;
            _prefabTileHeight = prefabHeight;
        }

        public void SetCornerOffsets(Vector3[] corners)
        {
            _cornersOffSet = corners;
        }    

        public void SetCorners()
        {
            _corners = new Vector3[_cornersOffSet.Length];

            for (int i = 0; i < _corners.Length; i++)
            {
                _corners[i] = _cornersOffSet[i] + transform.position;
            }
        }

        public void SetCoordinate(int coorx, int coorZ)
        {
            _slotCoordinate = new Vector2Int(coorx, coorZ);
        }

        public void GenerateTile()
        {
            DeleteTile();
            GameObject prefab = _map.GetTilePrefab(_terrainType);
            if (prefab != null)
            {
                GameObject tile = Instantiate(prefab, transform);
                tile.transform.localScale = new Vector3(_tileSize / _prefabTileSize, _tileHeight /_prefabTileHeight, _tileSize / _prefabTileSize);
                tile.GetComponent<PFTTile>().Init(_slotCoordinate, _tileSize);
            }
        }

        public void DeleteTile()
        {
            List<Transform> allchild = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                allchild.Add(transform.GetChild(i));
            }

            foreach (Transform trans in allchild)
            {
                DestroyImmediate(trans.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            if (_corners != null && _corners.Length > 0)
            {
                Gizmos.color = Color.gray;
                for (int i = 0; i < _corners.Length; i++)
                {
                    if (i > 0)
                    {
                        Gizmos.DrawLine(_corners[i - 1], _corners[i]);
                    }
                }

                Gizmos.DrawLine(_corners[_corners.Length - 1], _corners[0]);
            }
        }

    }
}