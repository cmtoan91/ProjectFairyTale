using System.Collections.Generic;
using UnityEngine;

public class PFTPlayerCardController : MonoBehaviour
{
    [SerializeField]
    int _numberOfCardOnHand = 0;
    
    [SerializeField]
    PFTCards _currentSelectedCard;

    [SerializeField]
    List<PFTCards> _allCardsOnHand = new List<PFTCards>();

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
        //Test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCardOnHand(_numberOfCardOnHand);
        }
    }


    void RegisterPlayer(object sender, params object[] args)
    {
        UIPlayerHand handui = (UIPlayerHand)sender;
        handui.RegisterPlayer(this);
    }


    void SpawnCardOnHand(int count)
    {
        _allCardsOnHand = new List<PFTCards>();

        for (int i = 0; i < count; i++)
        {
            _allCardsOnHand.Add(new PFTCards());
        }
    }

    void SubcribeToEvents()
    {
        Core.SubscribeEvent(EventType.HandUISpawn, RegisterPlayer);
    }

    void UnsubcribeToEvents()
    {
        Core.UnsubscribeEvent(EventType.HandUISpawn, RegisterPlayer);
    }

    private void OnDestroy()
    {
        UnsubcribeToEvents();
    }
}
