using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapDesigner {
    public class PFTTileSlot : MonoBehaviour
    {
        #region props
        [SerializeField]
        TileTerrainType _terrainType = TileTerrainType.Dirt;

        Vector3[] _corners;
        public Vector3[] Corners => _corners;

        PFTTileMapGenerator _generator;
        #endregion
        public void SetGenerator(PFTTileMapGenerator generator)
        {
            _generator = generator;
        }

        public void SetCorners(Vector3[] corners)
        {
            _corners = new Vector3[corners.Length];

            for (int i = 0; i < _corners.Length; i++)
            {
                _corners[i] = corners[i] + transform.position;
            }
        }

        public void GenerateTile()
        {
            DeleteTile();
            GameObject prefab = _generator.GetTilePrefab(_terrainType);
            if(prefab !=  null)
                Instantiate(prefab, transform);
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