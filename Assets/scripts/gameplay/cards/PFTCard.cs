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
            tile.DestroyUnit();
            GameObject unitObject = Instantiate(_info.UnitPrefab, tile.transform.position + tile.Height *0.55f* Vector3.up, Quaternion.identity);
            unitObject.transform.parent = tile.transform;
            PFTUnit unit = unitObject.GetComponent<PFTUnit>();
            tile.SetUnit(unit);
            unit.InitUnit(tile, _info.CardEffect, _info.UnitImage);
        }
    }
}