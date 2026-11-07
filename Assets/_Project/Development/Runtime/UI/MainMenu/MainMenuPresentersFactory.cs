#region

using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
using Runtime.UI.Core;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DataSaves.DataProviders;
using Runtime.Utility.DI;

#endregion

namespace Runtime.UI.MainMenu
{
    public class MainMenuPresentersFactory
    {
        private readonly DIContainer _container;

        public MainMenuPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public MainMenuScreenPresenter CreateMainMenuScreenPresenter(MainMenuScreenView view)
        {
            MainMenuScreenPresenter presenter = new(_container.Resolve<MainMenuPopupService>(),
                view,
                _container.Resolve<WalletService>(),
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<TypingMiniGameStatisticsService>(),
                _container.Resolve<GameConfigsProvider>().GetConfig<StatisticsConfig>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<PlayerDataProvider>());

            return presenter;
        }
    }
}