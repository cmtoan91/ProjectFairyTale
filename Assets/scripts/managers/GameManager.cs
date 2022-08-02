using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : IStateMachine<PFTGameStateType, PFTGameMessageType>
{
    public override PFTGameStateType UnassignedType => PFTGameStateType.Unassigned;

    private void Awake()
    {
        Init();
    }

    void Init()
    {

    }
}
