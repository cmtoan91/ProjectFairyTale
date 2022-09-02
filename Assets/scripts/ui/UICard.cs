using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MainGame 
{
    public class UICard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField]
        Image _cardImage;

        [SerializeField]
        Text _cardName;

        [SerializeField]
        GameObject _textPanel;

        [SerializeField]
        Vector3 _orginalPos;

        SO_CardInfo _cardInfo;
        public SO_CardInfo CardInfo => _cardInfo;

        RectTransform _rect;
        public void Init(SO_CardInfo cardInfo, Vector3 orgPos, RectTransform rect)
        {
            _cardInfo = cardInfo;
            _cardImage.sprite = cardInfo.CardImage;
            _cardName.text = cardInfo.CardName;
            _orginalPos = orgPos;
            _rect = rect;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _cardImage.color = new Color(0, 0, 0, 0);
            _textPanel.SetActive(false);
            Core.BroadcastEvent(EventType.OnCardSelected, this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _cardImage.color = new Color(1, 1, 1, 1);
            _textPanel.SetActive(true);
            _rect.anchoredPosition = _orginalPos;
            Core.BroadcastEvent(EventType.OnCardUnselected, this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
    }
}