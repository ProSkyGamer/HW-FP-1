#region

using System;
using System.Collections.Generic;
using Runtime.Gameplay.TypingMiniGame;
using Runtime.UI.Core;
using Runtime.UI.Gameplay;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Controllers;
using Runtime.Utility.DI;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Runtime.Gameplay.Bootstrap
{
    public class GameplaySceneRegistrations
    {
        public static void HandleRegistrations(DIContainer container)
        {
            container.AddAsSingle(CreateUIRoot).NonLazy();
            container.AddAsSingle(CreateGameplayScreenPresenter).NonLazy();

            container.AddAsSingle(CreateSceneState);
            container.AddAsSingle(CreateMiniGameConditionsHandler);
            container.AddAsSingle(CreateTypingMiniGameHandler);
            container.AddAsSingle(CreateGameplayPresentersFactory);
            container.AddAsSingle(CreateGameplayPopupService);
        }

        private static SceneState CreateSceneState(DIContainer c)
        {
            return new SceneState();
        }

        private static MiniGameConditionsHandler CreateMiniGameConditionsHandler(DIContainer c)
        {
            Func<string, string, bool> winConditions = (required, typed) => typed.Length >= required.Length && typed == required;
            Func<string, string, bool> loseConditions = (required, typed) => typed.Length >= required.Length && typed != required;

            MiniGameConditionsHandler miniGameConditionsHandler = new(winConditions, loseConditions);

            return miniGameConditionsHandler;
        }

        private static TypingMiniGameHandler CreateTypingMiniGameHandler(DIContainer c)
        {
            GameConfigsProvider gameConfigsProvider = c.Resolve<GameConfigsProvider>();
            TypingMiniGameConfig typingMiniGameConfig = gameConfigsProvider.GetConfig<TypingMiniGameConfig>();

            Dictionary<MiniGameType, StringGenerator> allStringGenerators = new();

            foreach (MiniGameType configuredMiniGameType in typingMiniGameConfig.GetConfiguredMiniGameTypes())
            {
                string miniGameSymbols = typingMiniGameConfig.GetMiniGameSymbols(configuredMiniGameType);
                allStringGenerators.Add(configuredMiniGameType, new StringGenerator(miniGameSymbols));
            }

            MiniGameConditionsHandler miniGameConditionsHandler = c.Resolve<MiniGameConditionsHandler>();
            int requiredStringLength = typingMiniGameConfig.GeneratedStringLength;
            ControllersFactory controllersFactory = c.Resolve<ControllersFactory>();
            StringTypingController stringTypingController = controllersFactory.CreateStringTypingController();

            TypingMiniGameHandler typingMiniGameHandler =
                new(allStringGenerators, miniGameConditionsHandler, requiredStringLength, stringTypingController);

            return typingMiniGameHandler;
        }

        private static GameplayUIRoot CreateUIRoot(DIContainer c)
        {
            ResourcesAssetsLoader assetsLoader = c.Resolve<ResourcesAssetsLoader>();
            GameObject uiRootPrefab = assetsLoader.Load<GameObject>("UI/Gameplay/GameplayUIRoot");
            GameObject instance = Object.Instantiate(uiRootPrefab);
            GameplayUIRoot gameplayUIRoot = instance.GetComponent<GameplayUIRoot>();

            if (gameplayUIRoot == null)
                throw new NullReferenceException("Prefab for MainMenuUIRoot does not contain required component");

            return gameplayUIRoot;
        }

        private static GameplayPopupService CreateGameplayPopupService(DIContainer c)
        {
            return new GameplayPopupService(c.Resolve<ViewsFactory>(),
                c.Resolve<ProjectPresentersFactory>(),
                c.Resolve<GameplayUIRoot>());
        }

        private static GameplayPresentersFactory CreateGameplayPresentersFactory(DIContainer c)
        {
            return new GameplayPresentersFactory(c);
        }

        private static GameplayScreenPresenter CreateGameplayScreenPresenter(DIContainer c)
        {
            GameplayUIRoot uiRoot = c.Resolve<GameplayUIRoot>();

            GameplayScreenView gameplayScreenView =
                c.Resolve<ViewsFactory>().CreateView<GameplayScreenView>(ViewIDs.GameplayScreenViewID, uiRoot.HUDLayer);

            GameplayPresentersFactory mainMenuPresentersFactory = c.Resolve<GameplayPresentersFactory>();

            GameplayScreenPresenter gameplayScreenPresenter = mainMenuPresentersFactory.CreateGameplayScreenPresenter(gameplayScreenView);
            gameplayScreenPresenter.Initialize();

            return gameplayScreenPresenter;
        }
    }
}