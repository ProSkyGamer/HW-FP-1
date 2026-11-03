#region

using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Gameplay.Bootstrap;
using Runtime.Meta.GameStatistics;
using Runtime.Utility.Bootstrap;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DI;
using Runtime.Utility.SceneManagement;
using UnityEngine;

#endregion

namespace Runtime.MainMenu
{
    public class MainMenuEntryPoint : SceneEntryPoint
    {
        private DIContainer _sceneContainer;

        private MiniGameChooseService _miniGameChooseService;
        private readonly List<IDisposable> _disposables = new();
        private GameInfoDataProvider _gameInfoDataProvider;
        private ResetStatisticsService _resetStatisticsService;

        public override void HandleSceneRegistrations(DIContainer container, ISceneArgs sceneArgs)
        {
            _sceneContainer = container;

            MainMenuSceneRegistrations.HandleRegistrations(_sceneContainer);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Main menu initialized");

            ControllersFactory controllersFactory = _sceneContainer.Resolve<ControllersFactory>();

            MultiInputTypeController<MiniGameType> miniGameTypeInputController = controllersFactory.CreateMultiInputTypeController(
                new Dictionary<MiniGameType, KeyCode>
                {
                    { MiniGameType.Numbers, KeyCode.Alpha1 },
                    { MiniGameType.Letters, KeyCode.Alpha2 }
                });

            _miniGameChooseService = new MiniGameChooseService(miniGameTypeInputController,
                _sceneContainer.Resolve<SceneSwitcherService>(),
                _sceneContainer.Resolve<ICoroutinesPerformer>());

            _disposables.Add(_miniGameChooseService);

            _gameInfoDataProvider = _sceneContainer.Resolve<GameInfoDataProvider>();
            _disposables.Add(_gameInfoDataProvider);

            _resetStatisticsService = _sceneContainer.Resolve<ResetStatisticsService>();
            _disposables.Add(_resetStatisticsService);

            yield return new WaitForSeconds(1f);
        }

        public override void StartScene()
        {
            Debug.Log("Main menu starting scene");

            _miniGameChooseService.Activate();
            _gameInfoDataProvider.Activate();
            _resetStatisticsService.Activate();
        }

        public override IEnumerator UnloadScene()
        {
            Debug.Log("Unloading main menu scene");

            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();

            yield break;
        }
    }
}