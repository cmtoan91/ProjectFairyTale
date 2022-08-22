using System.Collections.Generic;
using UnityEngine;
using UI;

namespace MainGame
{
    public class PFTPlayerCardController : MonoBehaviour
    {
        #region Props
        [SerializeField]
        int _numberOfCardOnHand = 0;

        [SerializeField]
        PFTCard _currentSelectedCard;

        [SerializeField]
        List<PFTCard> _allCardsInDeck = new List<PFTCard>();

        [SerializeField]
        List<PFTCard> _allCardsOnHand = new List<PFTCard>();

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

            List<PFTCard> toRemove = new List<PFTCard>();

            for(int i = 0; i < toDraw; i++)
            {
                PFTCard cardToDraw = _allCardsInDeck[_allCardsInDeck.Count - i - 1];
                SpawnCardOnHand(cardToDraw);
                toRemove.Add(cardToDraw);
            }

            for(int i = 0; i < toDraw; i++)
            {
                _allCardsInDeck.Remove(toRemove[i]);
            }

            _numberOfCardOnHand = _allCardsOnHand.Count;
        }


        public void SpawnCardOnHand(PFTCard card)
        {
            _allCardsOnHand.Add(card);
        }

        void SubcribeToEvents()
        {
            Core.SubscribeEvent(EventType.HandUISpawn, RegisterPlayer);
            Core.SubscribeEvent(EventType.DrawPhaseStart, DrawCards);
        }

        void UnsubcribeToEvents()
        {
            Core.UnsubscribeEvent(EventType.HandUISpawn, RegisterPlayer);
            Core.UnsubscribeEvent(EventType.DrawPhaseStart, DrawCards);
        }

        private void OnDestroy()
        {
            UnsubcribeToEvents();
        }
    }
}