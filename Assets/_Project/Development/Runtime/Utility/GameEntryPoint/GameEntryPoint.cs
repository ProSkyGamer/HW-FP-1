#region

using System.Collections;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Coroutines;
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

            ICoroutinesPerformer coroutinesPerformer = projectContainer.Resolve<ICoroutinesPerformer>();
            coroutinesPerformer.ProcessCoroutine(Initialize(projectContainer));
        }

        private IEnumerator Initialize(DIContainer container)
        {
            ILoadingScreen loadingScreen = container.Resolve<ILoadingScreen>();
            loadingScreen.Show();

            GameConfigsLoader gameConfigsLoader = container.Resolve<GameConfigsLoader>();

            yield return gameConfigsLoader.LoadAll();

            loadingScreen.Hide();

            SceneSwitcherService sceneSwitcherService = container.Resolve<SceneSwitcherService>();

            yield return sceneSwitcherService.SwitchSceneTo(Scenes.MainMenu);
        }
    }
}