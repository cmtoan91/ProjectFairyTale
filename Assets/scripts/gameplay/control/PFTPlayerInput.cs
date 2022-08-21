using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PFTPlayerInput : MonoBehaviour
    {
        [SerializeField]
        Camera _mainCam;

        [SerializeField]
        PFTTile _currentSelectedTile;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SelectTile();
            }
        }

        void SelectTile()
        {
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Collider col = hit.collider;
                PFTTile tile;
                if (col.TryGetComponent<PFTTile>(out tile))
                {
                    tile.SelectTile();
                    if (_currentSelectedTile != null)
                    {
                        _currentSelectedTile.UnselectTile();
                    }

                    _currentSelectedTile = tile;
                }
            }
        }
    }
}