#region

using System.Collections;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DataSaves.DataProviders;
using Runtime.Utility.DI;
using Runtime.Utility.LoadingScreen;
using Runtime.Utility.SceneManagement;
using UnityEngine;

#endregion

namespace Runtime.Utility.GameEntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            DIContainer projectContainer = new();

            GameEntryPointRegistrations.Register(projectContainer);
            projectContainer.Initialize();

            ICoroutinesPerformer coroutinesPerformer = projectContainer.Resolve<ICoroutinesPerformer>();
            coroutinesPerformer.ProcessCoroutine(Initialize(projectContainer));
        }

        private IEnumerator Initialize(DIContainer container)
        {
            ILoadingScreen loadingScreen = container.Resolve<ILoadingScreen>();
            loadingScreen.Show();

            GameConfigsProvider gameConfigsProvider = container.Resolve<GameConfigsProvider>();

            yield return gameConfigsProvider.LoadAll();

            PlayerDataProvider playerDataProvider = container.Resolve<PlayerDataProvider>();

            bool playerDataExists = false;

            yield return playerDataProvider.Exists(result => playerDataExists = result);

            if (playerDataExists)
                yield return playerDataProvider.Load();
            else
                playerDataProvider.Reset();

            loadingScreen.Hide();

            SceneSwitcherService sceneSwitcherService = container.Resolve<SceneSwitcherService>();

            yield return sceneSwitcherService.SwitchSceneTo(Scenes.MainMenu);
        }
    }
}