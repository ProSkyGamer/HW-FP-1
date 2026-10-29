#region

using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DI;
using Runtime.Utility.LoadingScreen;
using Runtime.Utility.SceneManagement;
using UnityEngine;

#endregion

namespace Runtime.Utility.GameEntryPoint
{
    public class GameEntryPointRegistrations
    {
        public static void Register(DIContainer container)
        {
            container.AddAsSingle(CreateResourcesAssetsLoader);
            container.AddAsSingle<IConfigsLoader>(CreateResourcesConfigLoader);
            container.AddAsSingle(CreateGameConfigsLoader);
            container.AddAsSingle<ICoroutinesPerformer>(CreateCoroutinesPerformer);

            container.AddAsSingle<ILoadingScreen>(CreateDefaultLoadingScreen);

            container.AddAsSingle(CreateSceneLoader);
            container.AddAsSingle<IControllersRunner>(CreateControllersRunner);
            container.AddAsSingle(CreateControllersFactory);
            container.AddAsSingle(CreateSceneSwitcherService);
        }

        private static ResourcesAssetsLoader CreateResourcesAssetsLoader(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = new();

            return resourcesAssetsLoader;
        }

        private static ResourcesConfigLoader CreateResourcesConfigLoader(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetLoader = c.Resolve<ResourcesAssetsLoader>();
            ResourcesConfigLoader resourcesConfigLoader = new(resourcesAssetLoader);

            return resourcesConfigLoader;
        }

        private static GameConfigsLoader CreateGameConfigsLoader(DIContainer c)
        {
            IConfigsLoader configsLoader = c.Resolve<IConfigsLoader>();
            GameConfigsLoader gameConfigsLoader = new(configsLoader);

            return gameConfigsLoader;
        }

        private static CoroutinesPerformer CreateCoroutinesPerformer(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetLoader = c.Resolve<ResourcesAssetsLoader>();
            CoroutinesPerformer coroutinesPerformerPrefab = resourcesAssetLoader.Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");
            CoroutinesPerformer coroutinesPerformer = Object.Instantiate(coroutinesPerformerPrefab);

            return coroutinesPerformer;
        }

        private static DefaultLoadingScreen CreateDefaultLoadingScreen(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetLoader = c.Resolve<ResourcesAssetsLoader>();
            DefaultLoadingScreen defaultLoadingScreenPrefab = resourcesAssetLoader.Load<DefaultLoadingScreen>("Utilities/DefaultLoadingScreen");
            DefaultLoadingScreen defaultLoadingScreen = Object.Instantiate(defaultLoadingScreenPrefab);

            return defaultLoadingScreen;
        }

        private static SceneLoader CreateSceneLoader(DIContainer c)
        {
            SceneLoader sceneLoader = new();

            return sceneLoader;
        }

        private static SceneSwitcherService CreateSceneSwitcherService(DIContainer c)
        {
            SceneLoader sceneLoader = c.Resolve<SceneLoader>();
            ILoadingScreen loadingScreen = c.Resolve<ILoadingScreen>();
            SceneSwitcherService sceneSwitcherService = new(sceneLoader, loadingScreen, c);

            return sceneSwitcherService;
        }

        private static ControllersesRunner CreateControllersRunner(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetLoader = c.Resolve<ResourcesAssetsLoader>();
            ControllersesRunner controllersRunnerPrefab = resourcesAssetLoader.Load<ControllersesRunner>("Utilities/ControllersRunner");
            ControllersesRunner controllersRunner = Object.Instantiate(controllersRunnerPrefab);

            return controllersRunner;
        }

        private static ControllersFactory CreateControllersFactory(DIContainer c)
        {
            IControllersRunner controllersRunner = c.Resolve<IControllersRunner>();
            ControllersFactory controllersFactory = new ControllersFactory(controllersRunner);

            return controllersFactory;
        }
    }
}