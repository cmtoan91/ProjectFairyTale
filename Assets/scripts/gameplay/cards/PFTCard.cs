using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class PFTCard : MonoBehaviour
    {
        #region props
        [Header("UI stuff")]
        [SerializeField]
        SpriteRenderer _cardImage;

        SO_CardInfo _info;
        #endregion
        public void InitCard(SO_CardInfo cardInfo)
        {
            _cardImage.sprite = cardInfo.CardImage;
            _info = cardInfo;
        }

        public void DeployUnit(PFTTile tile)
        {
            GameObject unitObject = Instantiate(_info.UnitPrefab, tile.transform.position, Quaternion.identity);
            unitObject.transform.parent = tile.transform;
            PFTUnit unit = unitObject.GetComponent<PFTUnit>();
            unit.InitUnit(tile, _info.CardEffect, _info.UnitImage);
        }
    }
}