using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PFTUnits : MonoBehaviour
    {
        PFTTile _currentTile;
        SO_CardEffect _effect;
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
    }
}