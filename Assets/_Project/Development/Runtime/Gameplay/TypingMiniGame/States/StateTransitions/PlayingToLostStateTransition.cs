namespace Runtime.Gameplay.TypingMiniGame.States.StateTransitions
{
    public class PlayingToLostStateTransition : IStateTransitionCondition
    {
        private readonly MiniGameConditionsHandler _miniGameConditionsHandler;
        private bool _isGameLost;

        public PlayingToLostStateTransition(MiniGameConditionsHandler miniGameConditionsHandler)
        {
            _miniGameConditionsHandler = miniGameConditionsHandler;
        }

        public TypingMiniGameState TransitioningState => TypingMiniGameState.Lost;

        public void InitializeStateTransition()
        {
            _miniGameConditionsHandler.Lost += MiniGameConditionsHandler_OnLost;
        }

        private void MiniGameConditionsHandler_OnLost()
        {
            _isGameLost = true;
        }

        public bool IsCanTransitionFrom(TypingMiniGameState state)
        {
            return state == TypingMiniGameState.Playing && _isGameLost;
        }

        public void Dispose()
        {
            _miniGameConditionsHandler.Lost -= MiniGameConditionsHandler_OnLost;
        }
    }
}