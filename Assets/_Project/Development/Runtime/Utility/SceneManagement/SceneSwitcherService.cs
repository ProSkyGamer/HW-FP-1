#region

using System;
using System.Collections;
using Runtime.Utility.Bootstrap;
using Runtime.Utility.DI;
using Runtime.Utility.LoadingScreen;
using Object = UnityEngine.Object;

#endregion

namespace Runtime.Utility.SceneManagement
{
    public class SceneSwitcherService
    {
        private readonly SceneLoader _sceneLoader;
        private readonly ILoadingScreen _loadingScreen;
        private readonly DIContainer _projectContainer;

        public SceneSwitcherService(SceneLoader sceneLoader, ILoadingScreen loadingScreen, DIContainer projectContainer)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _projectContainer = projectContainer;
        }

        public IEnumerator SwitchSceneTo(string sceneName, ISceneArgs sceneArgs = null)
        {
            _loadingScreen.Show();

            SceneEntryPoint currentSceneEntryPoint = Object.FindObjectOfType<SceneEntryPoint>();

            if (currentSceneEntryPoint != null)
                yield return currentSceneEntryPoint.UnloadScene();

            yield return _sceneLoader.LoadAsync(Scenes.Empty);

            yield return _sceneLoader.LoadAsync(sceneName);

            SceneEntryPoint loadedSceneEntryPoint = Object.FindObjectOfType<SceneEntryPoint>();

            if (loadedSceneEntryPoint == null)
                throw new NullReferenceException($"Scene {sceneName} does not has boostrap");

            DIContainer sceneContainer = new(_projectContainer);

            loadedSceneEntryPoint.HandleSceneRegistrations(sceneContainer, sceneArgs);

            yield return loadedSceneEntryPoint.Initialize();

            _loadingScreen.Hide();

            loadedSceneEntryPoint.StartScene();
        }
    }
}