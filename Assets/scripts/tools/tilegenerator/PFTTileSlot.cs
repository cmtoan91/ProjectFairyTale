using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapDesigner {
    public class PFTTileSlot : MonoBehaviour
    {
        #region props
        [SerializeField]
        TileTerrainType _terrainType = TileTerrainType.Dirt;

        float _tileSize;

        Vector3[] _cornersOffSet;
        public Vector3[] CornerOffSet => _cornersOffSet;

        Vector3[] _corners;
        public Vector3[] Corners => _corners;

        PFTTileMapGenerator _generator;
        #endregion
        public void SetGenerator(PFTTileMapGenerator generator)
        {
            _generator = generator;
        }

        public void SetSize(float size)
        {
            _tileSize = size;
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

        public void GenerateTile()
        {
            DeleteTile();
            GameObject prefab = _generator.GetTilePrefab(_terrainType);
            if (prefab != null)
            {
                GameObject tile = Instantiate(prefab, transform);
                tile.transform.localScale = new Vector3(_tileSize * 2, 1, _tileSize * 2);
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
            if (_corners != null)
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