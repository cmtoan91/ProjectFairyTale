using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PFTUnit : MonoBehaviour
    {
        #region props
        [SerializeField]
        Transform _visualImage;

        [SerializeField]
        Transform _mainCamera;

        PFTTile _currentTile;
        SO_CardEffect _effect;
        #endregion

        private void Start()
        {
            Core.BroadcastEvent(EventType.OnUnitSpawn, this);
        }

        private void Update()
        {
            FaceCamera(_mainCamera);
        }

        public void InitUnit(PFTTile tile, SO_CardEffect effect)
        {
            _currentTile = tile;
            _effect = effect;
        }

        public void MoveUnit(PFTTile tile)
        {
            _currentTile = tile;
        }

        public bool CheckIfMoveValid(PFTTile nextTile)
        {
            return true;
        }

        public void ActivateEffect()
        {
            _effect.ActivateEffect();
        }

        public void SetCamera(Transform cam)
        {
            _mainCamera = cam;
        }

        void FaceCamera(Transform campos)
        {
            if(campos == null)
            {
                return;
            }

            Vector3 dir = (campos.position - transform.position);
            dir.y = 0;
            dir = dir.normalized;

            if (_visualImage != null)
            {
                _visualImage.forward = dir;
            }
            else
            {
                Debug.Log("Visual Image not assigned");
            }
        }
    }
}