using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PFTPlayerUnitController : MonoBehaviour
    {
        #region props
        [SerializeField]
        PFTUnit _currentSelectedUnit;
        Camera _mainCamera;

        #endregion


        private void Start()
        {
            _mainCamera = Camera.main;
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 screenPos = Input.mousePosition;
                Ray ray = _mainCamera.ScreenPointToRay(screenPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
                {
                    Collider col = hit.collider;
                    PFTUnit unit = col.GetComponent<PFTUnit>();
                    if (unit != null)
                    {
                        SelectUnit(unit);
                    }
                    else
                    {
                        UnselectCurrentUnit();
                    }
                }
                else
                {
                    UnselectCurrentUnit();
                }
            }
        }

        void SelectUnit(PFTUnit unit)
        {
            if (_currentSelectedUnit != unit)
            {
                UnselectCurrentUnit();
            }
            _currentSelectedUnit = unit;
            _currentSelectedUnit.OnUnitSelected();
        }

        void UnselectCurrentUnit()
        {
            if (_currentSelectedUnit != null)
            {
                _currentSelectedUnit.OnUnitUnselected();
            }
        }
    }
}