using UnityEngine;
using MainGame;
using System.Collections.Generic;
namespace UI
{
    public class UIPlayerHand : MonoBehaviour
    {
        #region props
        [SerializeField]
        GameObject _cardUiPrefab;

        [Header("Debug")]
        [SerializeField]
        PFTPlayerCardController _currentPlayer;

        List<UICard> _allCardUIs = new List<UICard>();
        #endregion

        private void Awake()
        {
            SubcribeToEvents();
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            Core.BroadcastEvent(EventType.HandUISpawn, this);
        }

        public void RegisterPlayer(PFTPlayerCardController player)
        {
            _currentPlayer = player;
        }

        void DrawCardUI(SO_CardInfo cardInfo)
        {
            if(_cardUiPrefab == null)
            {
                Debug.LogError("Card prefab not assigned");
                return;
            }

            GameObject card = Instantiate(_cardUiPrefab, transform);
            UICard ui = card.GetComponent<UICard>();
            ui.Init(cardInfo);
        }

        void OnPlayerSpawn(object sender, params object[] args)
        {
            _currentPlayer = (PFTPlayerCardController)sender;
        }

        void OnDrawDone(object sender, params object[] args)
        {
            if(_currentPlayer != null)
            {
                for(int i = 0; i < _currentPlayer.NumberOfCardOnHand; i++)
                {
                    DrawCardUI(_currentPlayer.AllCardsOnHand[i]);
                }
            }
        }

        void SubcribeToEvents()
        {
            Core.SubscribeEvent(EventType.OnPlayerSpawn, OnPlayerSpawn);
            Core.SubscribeEvent(EventType.DrawDone, OnDrawDone);
        }

        void UnsubcribeToEvents()
        {
            Core.UnsubscribeEvent(EventType.OnPlayerSpawn, OnPlayerSpawn);
            Core.UnsubscribeEvent(EventType.DrawDone, OnDrawDone);
        }

        private void OnDestroy()
        {
            UnsubcribeToEvents();
        }
    }
}