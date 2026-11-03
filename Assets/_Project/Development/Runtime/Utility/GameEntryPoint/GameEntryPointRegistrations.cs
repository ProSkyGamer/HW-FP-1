#region

using System;
using System.Collections.Generic;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DataSaves;
using Runtime.Utility.DataSaves.DataKeys;
using Runtime.Utility.DataSaves.DataProviders;
using Runtime.Utility.DataSaves.Repository;
using Runtime.Utility.DataSaves.Serialization;
using Runtime.Utility.DI;
using Runtime.Utility.LoadingScreen;
using Runtime.Utility.Reacts;
using Runtime.Utility.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

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

            container.AddAsSingle(CreateWalletService).NonLazy();
            container.AddAsSingle(CreateTypingMiniGameStatisticsService).NonLazy();

            container.AddAsSingle<ISaveLoadDataService>(CreateDataSavesManager);
            container.AddAsSingle(CreatePlayerDataProvider).NonLazy();
            container.AddAsSingle<IDataKeysProvider>(CreateDataKeyStorage);
            container.AddAsSingle<IDataRepository>(CreateLocalFileDataRepository);
            container.AddAsSingle<IDataSerializer>(CreateJsonDataSerializer);
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

        private static GameConfigsProvider CreateGameConfigsLoader(DIContainer c)
        {
            IConfigsLoader configsLoader = c.Resolve<IConfigsLoader>();
            GameConfigsProvider gameConfigsProvider = new(configsLoader);

            return gameConfigsProvider;
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
            ControllersFactory controllersFactory = new(controllersRunner);

            return controllersFactory;
        }

        private static WalletService CreateWalletService(DIContainer c)
        {
            Dictionary<CurrencyType, ReactVariable<int>> currencies = new();

            foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
            {
                currencies.Add(currencyType, new ReactVariable<int>());
            }

            PlayerDataProvider playerDataProvider = c.Resolve<PlayerDataProvider>();

            WalletService walletService = new(currencies, playerDataProvider);

            return walletService;
        }

        private static TypingMiniGameStatisticsService CreateTypingMiniGameStatisticsService(DIContainer c)
        {
            TypingMiniGameStatisticsService typingMiniGameStatisticsService = new(c.Resolve<PlayerDataProvider>());

            return typingMiniGameStatisticsService;
        }

        private static DataSavesService CreateDataSavesManager(DIContainer c)
        {
            IDataKeysProvider keysStorage = c.Resolve<IDataKeysProvider>();
            IDataRepository dataRepository = c.Resolve<IDataRepository>();
            IDataSerializer serializer = c.Resolve<IDataSerializer>();

            DataSavesService dataSavesManager = new(keysStorage, dataRepository, serializer);

            return dataSavesManager;
        }

        private static DataKeyStorage CreateDataKeyStorage(DIContainer c)
        {
            DataKeyStorage dataKeyStorage = new();

            return dataKeyStorage;
        }

        private static LocalFileDataRepository CreateLocalFileDataRepository(DIContainer c)
        {
            string folderPath = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
            LocalFileDataRepository localFileDataRepository = new(folderPath, "json");

            return localFileDataRepository;
        }

        private static JsonDataSerializer CreateJsonDataSerializer(DIContainer c)
        {
            return new JsonDataSerializer();
        }

        private static PlayerDataProvider CreatePlayerDataProvider(DIContainer c)
        {
            PlayerDataProvider playerDataProvider = new(c.Resolve<ISaveLoadDataService>(), c.Resolve<GameConfigsProvider>());

            return playerDataProvider;
        }
    }
}