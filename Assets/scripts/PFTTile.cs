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

        #endregion
        public void Init(Vector2Int coor, float tileSize)
        {
            _coordinate = coor;
            _size = tileSize;
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
    }
}