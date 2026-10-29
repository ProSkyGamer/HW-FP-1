#region

using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Gameplay.TypingMiniGame;
using Runtime.Gameplay.TypingMiniGame.States;
using Runtime.Gameplay.TypingMiniGame.States.StatesImplementation;
using Runtime.Gameplay.TypingMiniGame.States.StateTransitions;
using Runtime.Utility.Bootstrap;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DI;
using Runtime.Utility.SceneManagement;
using UnityEngine;

#endregion

namespace Runtime.Gameplay.Bootstrap
{
    public class GameplayEntryPoint : SceneEntryPoint
    {
        private DIContainer _sceneContainer;

        private GameplaySceneArgs _sceneArgs;

        private TypingMiniGameHandler _typingMiniGameHandler;
        private ControllersFactory _controllersFactory;
        private TypingMiniGameStateController _typingMiniGameStateController;
        private SceneState _sceneState;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public override void HandleSceneRegistrations(DIContainer container, ISceneArgs sceneArgs)
        {
            _sceneContainer = container;

            if (sceneArgs is GameplaySceneArgs gameplaySceneArgs == false)
                throw new ArgumentException($"Passed wrong scene args for gameplay scene");

            _sceneArgs = gameplaySceneArgs;

            GameplaySceneRegistrations.HandleRegistrations(_sceneContainer);
        }

        public override IEnumerator Initialize()
        {
            _sceneState = _sceneContainer.Resolve<SceneState>();
            _controllersFactory = _sceneContainer.Resolve<ControllersFactory>();

            MiniGameConditionsHandler miniGameConditionsHandler = _sceneContainer.Resolve<MiniGameConditionsHandler>();

            _typingMiniGameHandler = _sceneContainer.Resolve<TypingMiniGameHandler>();
            _typingMiniGameHandler.SetGameType(_sceneArgs.MiniGameType);

            Dictionary<TypingMiniGameState, IMiniGameStateHandler> miniGameStateHandlers = GetMiniGameStateHandlers();

            List<IStateTransitionCondition> stateTransitionConditions = GetStateTransitionConditions(miniGameConditionsHandler);

            TypingMiniGameState startingState = TypingMiniGameState.WaitingForStart;

            _typingMiniGameStateController =
                _controllersFactory.CreateTypingMiniGameStateController(miniGameStateHandlers, stateTransitionConditions, startingState);

            _disposables.Add(_typingMiniGameStateController);

            yield return new WaitForSeconds(1f);
        }

        private List<IStateTransitionCondition> GetStateTransitionConditions(MiniGameConditionsHandler miniGameConditionsHandler)
        {
            return new List<IStateTransitionCondition>()
            {
                new WaitingForGameToStartTransition(_sceneState),
                new PlayingToWonStateTransition(miniGameConditionsHandler),
                new PlayingToLostStateTransition(miniGameConditionsHandler)
            };
        }

        private Dictionary<TypingMiniGameState, IMiniGameStateHandler> GetMiniGameStateHandlers()
        {
            return new()
            {
                { TypingMiniGameState.WaitingForStart, new WaitingForGameToStartState() },
                { TypingMiniGameState.Playing, new PlayingState(_typingMiniGameHandler) },
                {
                    TypingMiniGameState.Won,
                    new WonState(_sceneContainer.Resolve<SceneSwitcherService>(),
                        _controllersFactory.CreateInputKeyController(KeyCode.Space),
                        _sceneContainer.Resolve<ICoroutinesPerformer>())
                },
                {
                    TypingMiniGameState.Lost,
                    new LostState(_sceneContainer.Resolve<SceneSwitcherService>(),
                        _controllersFactory.CreateInputKeyController(KeyCode.Space),
                        _sceneContainer.Resolve<ICoroutinesPerformer>(), _sceneArgs)
                }
            };
        }

        public override void StartScene()
        {
            _typingMiniGameStateController.Activate();
            _sceneState.Start();
        }

        public override IEnumerator UnloadScene()
        {
            _typingMiniGameStateController.Deactivate();
            _typingMiniGameStateController.Destroy();

            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }

            _sceneState.Stop();

            yield break;
        }
    }
}