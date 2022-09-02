using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    [CreateAssetMenu(fileName = "cardinfo", menuName = "ScriptableObjects/Card Info", order = 1)]
    public class SO_CardInfo : ScriptableObject
    {
        [Header("Card Stuffs")]
        [SerializeField]
        Sprite _cardImage;
        public Sprite CardImage => _cardImage;

        [SerializeField]
        string _cardName;
        public string CardName => _cardName;

        [SerializeField]
        SO_CardEffect _cardEffect;
        public SO_CardEffect CardEffect => _cardEffect;

        [Header("Unit Info")]
        [SerializeField]
        Sprite _unitImage;
        public Sprite UnitImage => _unitImage;

        [SerializeField]
        GameObject _unitPrefab;
        public GameObject UnitPrefab => _unitPrefab;
    }
}