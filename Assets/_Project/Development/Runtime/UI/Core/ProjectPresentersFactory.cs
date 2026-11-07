#region

using System;
using Runtime.Gameplay.Bootstrap;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
using Runtime.UI.Common;
using Runtime.UI.MiniGameTypePopup;
using Runtime.UI.Wallet;
using Runtime.UI.WinLossCount;
using Runtime.Utility.AssetsLoader;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DI;
using Runtime.Utility.Reacts;
using Runtime.Utility.SceneManagement;

#endregion

namespace Runtime.UI.Core
{
    public class ProjectPresentersFactory
    {
        private readonly DIContainer _container;

        public ProjectPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public MiniGameTypePopupPresenter CreateMiniGameTypePopupPresenter(MiniGameTypePopupView view)
        {
            MiniGameTypePopupPresenter presenter = new(view,
                _container.Resolve<ViewsFactory>(),
                _container.Resolve<ProjectPresentersFactory>());

            return presenter;
        }

        public MiniGameTypeTilePresenter CreateMiniGameTypeTilePresenter(MiniGameTypeTileView view, MiniGameType miniGameType)
        {
            MiniGameTypeTilePresenter presenter = new(_container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>(),
                view,
                miniGameType);

            return presenter;
        }

        public CurrencyPresenter CreateCurrencyPresenter(IconTextElementView view, IReadonlyVariable<int> currency, CurrencyType currencyType)
        {
            CurrencyPresenter currencyPresenter =
                new(view,
                    currency,
                    currencyType,
                    _container.Resolve<GameConfigsProvider>().GetConfig<CurrencyIconConfig>());

            return currencyPresenter;
        }

        public WalletPresenter CreateWalletPresenter(IconTextElementListView view)
        {
            WalletPresenter walletPresenter = new(view,
                _container.Resolve<WalletService>(),
                _container.Resolve<ViewsFactory>(),
                _container.Resolve<ProjectPresentersFactory>());

            return walletPresenter;
        }

        public CountTextPresenter CreateCountTextPresenter(DoubleTextView view, IReadonlyVariable<int> countVariable, string name)
        {
            CountTextPresenter countTextPresenter = new(countVariable, name, view);

            return countTextPresenter;
        }

        public WinLossCountPresenter CreateWinLossCountPresenter(WinLossCountView view)
        {
            WinLossCountPresenter winLossCountPresenter = new(view,
                _container.Resolve<TypingMiniGameStatisticsService>(),
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<ViewsFactory>());

            return winLossCountPresenter;
        }

        public ConfirmCancelPopupPresenter CreateConfirmCancelPopupPresenter(ConfirmCancelPopupView view, Action confirmCallback, string popupText)
        {
            ConfirmCancelPopupPresenter confirmCancelPopupPresenter = new(view, confirmCallback, popupText);

            return confirmCancelPopupPresenter;
        }

        public ConfirmPopupPresenter CreateConfirmPopupPresenter(ConfirmPopupView view, string popupText)
        {
            ConfirmPopupPresenter confirmPopupPresenter = new(view, popupText);

            return confirmPopupPresenter;
        }
    }
}