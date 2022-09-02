using System.Collections.Generic;
using UnityEngine;
using UI;

namespace MainGame
{
    public class PFTPlayerCardController : MonoBehaviour
    {
        #region Props
        [SerializeField]
        PFTCard _currentSelectedCard;

        [SerializeField]
        List<SO_CardInfo> _allCardsInDeck = new List<SO_CardInfo>();

        [SerializeField]
        List<SO_CardInfo> _allCardsOnHand = new List<SO_CardInfo>();
        public List<SO_CardInfo> AllCardsOnHand => _allCardsOnHand;

        int _numberOfCardOnHand = 0;
        public int NumberOfCardOnHand => _numberOfCardOnHand;
        #endregion
        private void Awake()
        {
            SubcribeToEvents();
        }

        private void Start()
        {
            Core.BroadcastEvent(EventType.OnPlayerSpawn, this);
        }

        void RegisterPlayer(object sender, params object[] args)
        {
            UIPlayerHand handui = (UIPlayerHand)sender;
            handui.RegisterPlayer(this);
        }

        void DrawCards(object sender, params object[] args)
        {
            int noOfCard = (int)args[0];
            int toDraw = noOfCard;
            if(_allCardsInDeck.Count < toDraw)
            {
                toDraw = _allCardsInDeck.Count;
            }

            List<SO_CardInfo> toRemove = new List<SO_CardInfo>();

            for(int i = 0; i < toDraw; i++)
            {
                SO_CardInfo cardToDraw = _allCardsInDeck[_allCardsInDeck.Count - i - 1];
                SpawnCardOnHand(cardToDraw);
                toRemove.Add(cardToDraw);
            }

            for(int i = 0; i < toDraw; i++)
            {
                _allCardsInDeck.Remove(toRemove[i]);
            }

            _numberOfCardOnHand = _allCardsOnHand.Count;
            Core.BroadcastEvent(EventType.DrawDone, this);
        }


        public void SpawnCardOnHand(SO_CardInfo card)
        {
            _allCardsOnHand.Add(card);
        }

        void SubcribeToEvents()
        {
            Core.SubscribeEvent(EventType.HandUISpawn, RegisterPlayer);
            Core.SubscribeEvent(EventType.DrawStart, DrawCards);
        }

        void UnsubcribeToEvents()
        {
            Core.UnsubscribeEvent(EventType.HandUISpawn, RegisterPlayer);
            Core.UnsubscribeEvent(EventType.DrawStart, DrawCards);
        }

        private void OnDestroy()
        {
            UnsubcribeToEvents();
        }
    }
}