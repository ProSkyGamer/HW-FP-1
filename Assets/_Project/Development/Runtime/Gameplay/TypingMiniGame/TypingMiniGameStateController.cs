#region

using System;
using System.Collections.Generic;
using Runtime.Gameplay.TypingMiniGame.States;
using Runtime.Gameplay.TypingMiniGame.States.StateTransitions;
using Runtime.Utility.Controllers;
using UnityEngine;

#endregion

namespace Runtime.Gameplay.TypingMiniGame
{
    public class TypingMiniGameStateController : Controller, IDisposable
    {
        private readonly Dictionary<TypingMiniGameState, IMiniGameStateHandler> _miniGameStateHandlers;
        private readonly List<IStateTransitionCondition> _transitionConditions;

        private TypingMiniGameState _currentState;
        private readonly TypingMiniGameState _startingState;

        public TypingMiniGameStateController(Dictionary<TypingMiniGameState, IMiniGameStateHandler> miniGameStateHandlers,
            List<IStateTransitionCondition> transitionConditions, TypingMiniGameState startingState)
        {
            _miniGameStateHandlers = miniGameStateHandlers;
            _transitionConditions = transitionConditions;

            _startingState = startingState;
        }

        public void Initialize()
        {
            foreach (IStateTransitionCondition condition in _transitionConditions)
            {
                condition.InitializeStateTransition();
            }

            _currentState = _startingState;

            IMiniGameStateHandler newStateHandler = GetHandlerFor(_currentState);
            newStateHandler.TransitionIntoThis();
        }

        protected override void ProcessUpdateInternal(float deltaTime)
        {
            foreach (IStateTransitionCondition stateTransitionCondition in _transitionConditions)
            {
                if (stateTransitionCondition.IsCanTransitionFrom(_currentState) == false)
                    continue;

                ProcessStateChangeTo(stateTransitionCondition.TransitioningState);

                break;
            }
        }

        private void ProcessStateChangeTo(TypingMiniGameState state)
        {
            Debug.Log($"Transitioning from {_currentState} to {state}");

            IMiniGameStateHandler previousStateHandler = GetHandlerFor(_currentState);
            previousStateHandler.TransitionOutOfThis();

            _currentState = state;

            IMiniGameStateHandler newStateHandler = GetHandlerFor(_currentState);
            newStateHandler.TransitionIntoThis();
        }

        private IMiniGameStateHandler GetHandlerFor(TypingMiniGameState state)
        {
            if (_miniGameStateHandlers.TryGetValue(state, out IMiniGameStateHandler stateHandler) == false)
                throw new NullReferenceException($"No handler for state {state}");

            return stateHandler;
        }

        public void Dispose()
        {
            foreach (IStateTransitionCondition condition in _transitionConditions)
            {
                condition.Dispose();
            }

            IMiniGameStateHandler currentStateHandler = GetHandlerFor(_currentState);

            currentStateHandler.TransitionOutOfThis();
        }
    }
}