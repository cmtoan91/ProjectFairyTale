using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MainGame
{
    public class GameManager : IStateMachine<PFTGameStateType, PFTGameMessageType>
    {
        #region Serialized Properties
        [SerializeField]
        int _cardDrawnPerPhase = 3;
        public int CardsDrawnPerPhase => _cardDrawnPerPhase;
        #endregion
        public override PFTGameStateType UnassignedType => PFTGameStateType.Unassigned;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("draw");
                ChangeState(PFTGameStateType.DrawPhase);
            }
        }

        void Init()
        {
            Type enumType = Type.GetType("PFTGameStateType");
            PFTGameStateType[] allGameStateType = Enum.GetValues(enumType) as PFTGameStateType[];
            foreach (PFTGameStateType gameStateType in allGameStateType)
            {
                string stateName = string.Format("MainGame.PFT{0}GameState", gameStateType);
                Type stateType = Type.GetType(stateName);

                if (stateType != null)
                {
                    IState state = Activator.CreateInstance(stateType, new object[] { this }) as GameState;
                    Debug.Log(state);
                    if (state == null)
                        throw new Exception(stateName + " is not a valid state or missing.");
                    else
                        RegisterState(state);
                }
            }
        }

        public abstract class GameState : IState
        {
            protected GameManager Manager;
            public GameState(GameManager manager) : base(manager)
            {
                Manager = manager;
            }
        }
    }

}