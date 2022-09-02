namespace MainGame
{
    public class PFTDrawPhaseGameState: GameManager.GameState
    {
        public PFTDrawPhaseGameState(GameManager manager) : base(manager) { }

        public override PFTGameStateType Type => PFTGameStateType.DrawPhase;

        public override void OnStateEnter(PFTGameStateType prevStateType, object[] args)
        {
            Core.BroadcastEvent(EventType.DrawStart, Manager, Manager.CardsDrawnPerPhase);
        }
    }
}
