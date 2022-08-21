using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class PFTCards : MonoBehaviour
    {
        #region props
        [Header("UI stuff")]
        Image _cardImage;

        [SerializeField]
        Text _cardName;

        SO_CardInfo _info;
        #endregion
        public void InitCard(SO_CardInfo cardInfo)
        {
            _cardImage.sprite = cardInfo.CardImage;
            _cardName.text = cardInfo.CardName;
            _info = cardInfo;
        }

        public void DeployUnit(PFTTile tile)
        {
            GameObject unitObject = Instantiate(_info.UnitPrefab, tile.transform);
            PFTUnits unit = unitObject.GetComponent<PFTUnits>();
            unit.InitUnit(tile, _info.CardEffect);
        }
    }
}