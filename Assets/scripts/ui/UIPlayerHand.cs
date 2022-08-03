using System.Collections;

using UnityEngine;

public class UIPlayerHand : MonoBehaviour
{
    #region props
    [SerializeField]
    GameObject _cardPrefab;

    [Header("Debug")]
    [SerializeField]
    PFTPlayerCardController _currentPlayer;

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

    void OnPlayerSpawn(object sender, params object[] args)
    {
        _currentPlayer = (PFTPlayerCardController)sender;
    }

    void SubcribeToEvents()
    {
        Core.SubscribeEvent(EventType.OnPlayerSpawn, OnPlayerSpawn);
    }

    void UnsubcribeToEvents()
    {
        Core.UnsubscribeEvent(EventType.OnPlayerSpawn, OnPlayerSpawn);
    }

    private void OnDestroy()
    {
        UnsubcribeToEvents();
    }
}
