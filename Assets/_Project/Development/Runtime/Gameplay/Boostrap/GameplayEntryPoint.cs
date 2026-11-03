#region

using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.Gameplay.TypingMiniGame;
using Runtime.Gameplay.TypingMiniGame.States;
using Runtime.Gameplay.TypingMiniGame.States.StatesImplementation;
using Runtime.Gameplay.TypingMiniGame.States.StateTransitions;
using Runtime.Meta.GameStatistics;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Bootstrap;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DataSaves.DataProviders;
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
        private PlayerDataProvider _playerDataProvider;

        private readonly List<IDisposable> _disposables = new();

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
            _playerDataProvider = _sceneContainer.Resolve<PlayerDataProvider>();

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
            return new List<IStateTransitionCondition>
            {
                new WaitingForGameToStartTransition(_sceneState),
                new PlayingToWonStateTransition(miniGameConditionsHandler),
                new PlayingToLostStateTransition(miniGameConditionsHandler)
            };
        }

        private Dictionary<TypingMiniGameState, IMiniGameStateHandler> GetMiniGameStateHandlers()
        {
            SceneSwitcherService sceneSwitcherService = _sceneContainer.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = _sceneContainer.Resolve<ICoroutinesPerformer>();
            TypingMiniGameConfig typingMiniGameConfig = _sceneContainer.Resolve<GameConfigsProvider>().GetConfig<TypingMiniGameConfig>();
            MiniGameType miniGameType = _sceneArgs.MiniGameType;
            WalletService walletService = _sceneContainer.Resolve<WalletService>();
            TypingMiniGameStatisticsService statisticsService = _sceneContainer.Resolve<TypingMiniGameStatisticsService>();

            WonState wonState = new(sceneSwitcherService,
                _controllersFactory.CreateInputKeyController(KeyCode.Space),
                coroutinesPerformer,
                walletService,
                typingMiniGameConfig.GetDeltaCurrencyOnWin(miniGameType),
                statisticsService);

            LostState lostState = new(sceneSwitcherService,
                _controllersFactory.CreateInputKeyController(KeyCode.Space),
                coroutinesPerformer,
                _sceneArgs,
                walletService,
                typingMiniGameConfig.GetDeltaCurrencyOnLose(miniGameType),
                statisticsService);

            return new Dictionary<TypingMiniGameState, IMiniGameStateHandler>
            {
                { TypingMiniGameState.WaitingForStart, new WaitingForGameToStartState() },
                { TypingMiniGameState.Playing, new PlayingState(_typingMiniGameHandler) },
                { TypingMiniGameState.Won, wonState },
                { TypingMiniGameState.Lost, lostState }
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

            yield return _playerDataProvider.Save();

            yield break;
        }
    }
}