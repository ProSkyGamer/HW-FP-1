#region

using System;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
using Runtime.UI.Core;
using Runtime.UI.MainMenu;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DataSaves.DataProviders;
using Runtime.Utility.DI;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Runtime.MainMenu
{
    public class MainMenuSceneRegistrations
    {
        public static void HandleRegistrations(DIContainer container)
        {
            Debug.Log("Main menu scene registrations");

            container.AddAsSingle(CreateMainMenuUIRoot).NonLazy();
            container.AddAsSingle(CreateMainMenuScreenPresenter).NonLazy();

            container.AddAsSingle(CreateWalletInfoDataProvider);
            container.AddAsSingle(CreateResetStatisticsService);

            container.AddAsSingle(CreateMainMenuPopupService);
            container.AddAsSingle(CreateMainMenuPresentersFactory);
        }

        private static GameInfoDataProvider CreateWalletInfoDataProvider(DIContainer c)
        {
            InputKeyController inputKeyController = c.Resolve<ControllersFactory>().CreateInputKeyController(KeyCode.I);

            GameInfoDataProvider gameInfoDataProvider = new(c.Resolve<WalletService>(),
                c.Resolve<TypingMiniGameStatisticsService>(),
                inputKeyController);

            return gameInfoDataProvider;
        }

        private static ResetStatisticsService CreateResetStatisticsService(DIContainer c)
        {
            InputKeyController inputKeyController = c.Resolve<ControllersFactory>().CreateInputKeyController(KeyCode.R);

            ResetStatisticsService resetStatisticsService = new(c.Resolve<GameConfigsProvider>().GetConfig<StatisticsConfig>(),
                c.Resolve<WalletService>(),
                c.Resolve<TypingMiniGameStatisticsService>(),
                inputKeyController,
                c.Resolve<ICoroutinesPerformer>(),
                c.Resolve<PlayerDataProvider>());

            return resetStatisticsService;
        }

        private static MainMenuUIRoot CreateMainMenuUIRoot(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            GameObject mainMenuUIRootPrefab = resourcesAssetsLoader.Load<GameObject>("UI/MainMenu/MainMenuUIRoot");
            GameObject instance = Object.Instantiate(mainMenuUIRootPrefab);
            MainMenuUIRoot mainMenuUIRoot = instance.GetComponent<MainMenuUIRoot>();

            if (mainMenuUIRoot == null)
                throw new NullReferenceException("Prefab for MainMenuUIRoot does not contain required component");

            return mainMenuUIRoot;
        }

        private static MainMenuPopupService CreateMainMenuPopupService(DIContainer c)
        {
            return new MainMenuPopupService(c.Resolve<ViewsFactory>(),
                c.Resolve<ProjectPresentersFactory>(),
                c.Resolve<MainMenuUIRoot>());
        }

        private static MainMenuPresentersFactory CreateMainMenuPresentersFactory(DIContainer c)
        {
            return new MainMenuPresentersFactory(c);
        }

        private static MainMenuScreenPresenter CreateMainMenuScreenPresenter(DIContainer c)
        {
            MainMenuUIRoot uiRoot = c.Resolve<MainMenuUIRoot>();

            MainMenuScreenView mainMenuScreenView =
                c.Resolve<ViewsFactory>().CreateView<MainMenuScreenView>(ViewIDs.MainMenuScreenViewID, uiRoot.HUDLayer);

            MainMenuPresentersFactory mainMenuPresentersFactory = c.Resolve<MainMenuPresentersFactory>();

            MainMenuScreenPresenter mainMenuScreenPresenter = mainMenuPresentersFactory.CreateMainMenuScreenPresenter(mainMenuScreenView);
            mainMenuScreenPresenter.Initialize();

            return mainMenuScreenPresenter;
        }
    }
}