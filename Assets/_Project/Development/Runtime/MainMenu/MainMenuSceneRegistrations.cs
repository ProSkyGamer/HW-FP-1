#region

using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DataSaves.DataProviders;
using Runtime.Utility.DI;
using UnityEngine;

#endregion

namespace Runtime.MainMenu
{
    public class MainMenuSceneRegistrations
    {
        public static void HandleRegistrations(DIContainer container)
        {
            Debug.Log("Main menu scene registrations");

            container.AddAsSingle(CreateWalletInfoDataProvider);
            container.AddAsSingle(CreateResetStatisticsService);
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
    }
}