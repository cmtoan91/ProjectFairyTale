using System.Collections;
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

}
