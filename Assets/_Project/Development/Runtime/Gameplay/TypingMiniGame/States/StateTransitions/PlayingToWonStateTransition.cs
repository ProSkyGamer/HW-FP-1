namespace Runtime.Gameplay.TypingMiniGame.States.StateTransitions
{
    public class PlayingToWonStateTransition : IStateTransitionCondition
    {
        private readonly MiniGameConditionsHandler _miniGameConditionsHandler;
        private bool _isGameWon;

        public PlayingToWonStateTransition(MiniGameConditionsHandler miniGameConditionsHandler)
        {
            _miniGameConditionsHandler = miniGameConditionsHandler;
        }

        public TypingMiniGameState TransitioningState => TypingMiniGameState.Won;

        public void InitializeStateTransition()
        {
            _miniGameConditionsHandler.Won += MiniGameConditionsHandler_OnWon;
        }

        private void MiniGameConditionsHandler_OnWon()
        {
            _isGameWon = true;
        }

        public bool IsCanTransitionFrom(TypingMiniGameState state)
        {
            return state == TypingMiniGameState.Playing && _isGameWon;
        }

        public void Dispose()
        {
            _miniGameConditionsHandler.Won -= MiniGameConditionsHandler_OnWon;
        }
    }
}