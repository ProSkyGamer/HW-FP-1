#region

using System;

#endregion

namespace Runtime.Gameplay.TypingMiniGame.States.StateTransitions
{
    public interface IStateTransitionCondition : IDisposable
    {
        public void InitializeStateTransition();

        public TypingMiniGameState TransitioningState { get; }
        public bool IsCanTransitionFrom(TypingMiniGameState state);
    }
}