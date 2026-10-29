#region

using System;
using System.Collections.Generic;
using Runtime.Gameplay.TypingMiniGame;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Controllers;
using Runtime.Utility.DI;

#endregion

namespace Runtime.Gameplay.Bootstrap
{
    public class GameplaySceneRegistrations
    {
        public static void HandleRegistrations(DIContainer container)
        {
            container.AddAsSingle(CreateSceneState);
            container.AddAsSingle(CreateMiniGameConditionsHandler);
            container.AddAsSingle(CreateTypingMiniGameHandler);
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
            GameConfigsLoader gameConfigsLoader = c.Resolve<GameConfigsLoader>();
            TypingMiniGameConfig typingMiniGameConfig = gameConfigsLoader.GetConfig<TypingMiniGameConfig>();

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
    }
}