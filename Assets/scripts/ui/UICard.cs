using UnityEngine;
using UnityEngine.UI;

namespace MainGame {
    public class UICard : MonoBehaviour
    {
        [SerializeField]
        Image _cardImage;

        [SerializeField]
        Text _cardName;

        SO_CardInfo _cardInfo;

        public void Init(SO_CardInfo cardInfo)
        {
            _cardInfo = cardInfo;
            _cardImage.sprite = cardInfo.CardImage;
            _cardName.text = cardInfo.CardName;
        }

    }
}