#region

using Runtime.Gameplay.Bootstrap;

#endregion

namespace Runtime.Gameplay.TypingMiniGame.States.StateTransitions
{
    public class WaitingForGameToStartTransition : IStateTransitionCondition
    {
        private readonly SceneState _sceneState;

        public WaitingForGameToStartTransition(SceneState sceneState)
        {
            _sceneState = sceneState;
        }

        public void InitializeStateTransition()
        {
        }

        public TypingMiniGameState TransitioningState => TypingMiniGameState.Playing;

        public bool IsCanTransitionFrom(TypingMiniGameState state)
        {
            return _sceneState.IsPlaying && state == TypingMiniGameState.WaitingForStart;
        }

        public void Dispose()
        {
        }
    }
}