using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class PFTCards : MonoBehaviour
    {
        #region props
        [Header("UI Stuffs")]
        [SerializeField]
        Image _cardImage;

        [SerializeField]
        string _cardName;

        [SerializeField]
        SO_CardEffect _cardEffects;


        PFTTile _currentTile;
        #endregion
        void MoveCard(PFTTile tile)
        {
            if (CheckIfMoveValid(tile))
            {
                _currentTile = tile;
            }
        }

        bool CheckIfMoveValid(PFTTile nextTile)
        {
            return true;
        }

        void InitCard()
        {

        }
    }
}