using System.Collections.Generic;
using UnityEngine;
using UI;

namespace MainGame
{
    public class PFTPlayerCardController : MonoBehaviour
    {
        #region Props
        [SerializeField]
        GameObject _3dCardPrefab;


        [Header("Debug")]
        [SerializeField]
        List<SO_CardInfo> _allCardsInDeck = new List<SO_CardInfo>();

        [SerializeField]
        List<SO_CardInfo> _allCardsOnHand = new List<SO_CardInfo>();

        [SerializeField]
        PFTCard _currentSelectedCard;

        [SerializeField]
        PFTTile _currentSelectedTile;

        public List<SO_CardInfo> AllCardsOnHand => _allCardsOnHand;

        int _numberOfCardOnHand = 0;
        public int NumberOfCardOnHand => _numberOfCardOnHand;
        Camera _mainCamera;

        #endregion
        private void Awake()
        {
            SubcribeToEvents();
        }

        private void Start()
        {
            Core.BroadcastEvent(EventType.OnPlayerSpawn, this);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                MoveCard();
            }

            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                //Cursor.visible = true;
            }
        }

        public void SetMainCam(Camera cam)
        {
            _mainCamera = cam;
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

        void SelectCard(object sender, params object[] args)
        {
            UICard ui = (UICard)sender;
            GameObject cardObj = Instantiate(_3dCardPrefab, Vector3.zero, Quaternion.identity);
            PFTCard card = cardObj.GetComponent<PFTCard>();
            card.InitCard(ui.CardInfo);
            _currentSelectedCard = card;
        }

        void UnselectCard(object sender, params object[] args)
        {
            if (_currentSelectedCard == null)
                return;

            if(_currentSelectedTile != null)
            {
                _currentSelectedCard.DeployUnit(_currentSelectedTile);
            }
            Destroy(_currentSelectedCard.gameObject);
            _currentSelectedCard = null;
        }

        void MoveCard()
        {
            if (_currentSelectedCard == null)
                return;

            Vector3 screenPos = Input.mousePosition;
            screenPos.z = 10f;
            Ray ray = _mainCamera.ScreenPointToRay(screenPos);
            Vector3 dir = ray.direction;

            float h = _mainCamera.transform.position.y - 1;
            float angle = Vector3.Angle(Vector3.down, dir) * Mathf.Deg2Rad;
            float mag = h / Mathf.Cos(angle);
            Vector3 worldPos = _mainCamera.transform.position + dir * mag;

            _currentSelectedCard.transform.position = worldPos + new Vector3(2, 0, 3);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Collider col = hit.collider;
                PFTTile tile;
                if (col.GetComponentInChildren<PFTTile>())
                {
                    tile = col.GetComponentInChildren<PFTTile>();
                    if (_currentSelectedTile != null)
                    {
                        if (_currentSelectedTile == tile)
                            return;

                        _currentSelectedTile.UnselectTile();
                    }
                    tile.SelectTile();
                    _currentSelectedTile = tile;
                }
            }
            else
            {
                if(_currentSelectedTile != null)
                {
                    _currentSelectedTile.UnselectTile();
                }
                _currentSelectedTile = null;
            }
            //Cursor.visible = false;
        }

        public void SpawnCardOnHand(SO_CardInfo card)
        {
            _allCardsOnHand.Add(card);
        }

        void SubcribeToEvents()
        {
            Core.SubscribeEvent(EventType.HandUISpawn, RegisterPlayer);
            Core.SubscribeEvent(EventType.DrawStart, DrawCards);
            Core.SubscribeEvent(EventType.OnCardSelected, SelectCard);
            Core.SubscribeEvent(EventType.OnCardUnselected, UnselectCard);
        }

        void UnsubcribeToEvents()
        {
            Core.UnsubscribeEvent(EventType.HandUISpawn, RegisterPlayer);
            Core.UnsubscribeEvent(EventType.DrawStart, DrawCards);
            Core.UnsubscribeEvent(EventType.OnCardSelected, SelectCard);
            Core.UnsubscribeEvent(EventType.OnCardUnselected, UnselectCard);
        }

        private void OnDestroy()
        {
            UnsubcribeToEvents();
        }
    }
}