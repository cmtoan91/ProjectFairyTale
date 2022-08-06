using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFTTile : MonoBehaviour
{
    #region props
    [SerializeField]
    TileTerrainType _terrainType = TileTerrainType.Dirt;

    MeshRenderer _meshRenderer;
    Color orgColor;

    #endregion
    public void Init(MeshRenderer renderer)
    {
        _meshRenderer = renderer;
        orgColor = renderer.material.color;
    }
    
    public void SelectTile()
    {
        _meshRenderer.material.color = Color.blue;
    }

    public void UnselectTile()
    {
        _meshRenderer.material.color = orgColor;
    }

}
