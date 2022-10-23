using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PFTTile : MonoBehaviour
    {
        #region props
        [SerializeField]
        TileTerrainType _terrainType = TileTerrainType.Dirt;

        [SerializeField]
        float _size;

        [SerializeField]
        float _height = 1;

        [SerializeField]
        Vector2Int _coordinate;

        [SerializeField]
        MeshRenderer _meshRenderer;
        Color orgColor;

        PFTUnit _currentUnit;

        public PFTUnit CurrentUnit => _currentUnit;

        public float Height => _height;
        #endregion
        public void Init(Vector2Int coor, float tileSize, float height)
        {
            _coordinate = coor;
            _size = tileSize;
            _height = height;
        }

        public void SelectTile()
        {
            orgColor = _meshRenderer.material.color;
            _meshRenderer.material.color = Color.blue;
        }

        public void UnselectTile()
        {
            _meshRenderer.material.color = orgColor;
        }

        public void SetUnit(PFTUnit unit)
        {
            _currentUnit = unit;
        }

        public void DestroyUnit()
        {
            if (_currentUnit != null)
            {
                Destroy(_currentUnit.gameObject);
                _currentUnit = null;
            }
        }

        private void OnValidate()
        {
            //for (int i = 0; i < transform.childCount; i++)
            //{
            //    if (transform.GetChild(i).name != "HexBase")
            //    {
            //        _meshRenderer = transform.GetChild(i).GetComponent<MeshRenderer>();
            //    }
            //}

            if (GetComponentInParent<MapDesigner.PFTTileSlot>())
            {
                _height = GetComponentInParent<MapDesigner.PFTTileSlot>().TileHeight;
            }
        }
    }
}