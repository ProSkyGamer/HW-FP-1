#region

using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Meta.GameStatistics;
using Runtime.Utility.Bootstrap;
using Runtime.Utility.DI;
using UnityEngine;

#endregion

namespace Runtime.MainMenu
{
    public class MainMenuEntryPoint : SceneEntryPoint
    {
        private DIContainer _sceneContainer;

        private readonly List<IDisposable> _disposables = new();
        private ResetStatisticsService _resetStatisticsService;

        public override void HandleSceneRegistrations(DIContainer container, ISceneArgs sceneArgs)
        {
            _sceneContainer = container;

            MainMenuSceneRegistrations.HandleRegistrations(_sceneContainer);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Main menu initialized");

            _resetStatisticsService = _sceneContainer.Resolve<ResetStatisticsService>();
            _disposables.Add(_resetStatisticsService);

            yield return new WaitForSeconds(1f);
        }

        public override void StartScene()
        {
            Debug.Log("Main menu starting scene");

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