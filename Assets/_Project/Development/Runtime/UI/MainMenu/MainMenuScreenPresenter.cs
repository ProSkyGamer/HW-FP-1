#region

using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
using Runtime.UI.Core;
using Runtime.UI.Wallet;
using Runtime.UI.WinLossCount;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DataSaves.DataProviders;

#endregion

namespace Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private const string ResetNotificationFormatString = "Вы хотите потратить {0} {1}, чтобы сбросить количество побед и поражений?";
        private const string FailedResetNotificationFormatString = "Недостаточно средств! Необходимо {0} {1}, у вас есть только {2} {1}";

        private readonly MainMenuPopupService _popupService;
        private readonly MainMenuScreenView _view;
        private readonly WalletService _walletService;
        private readonly ProjectPresentersFactory _projectPresentersFactory;
        private readonly TypingMiniGameStatisticsService _statisticsService;
        private readonly StatisticsConfig _statisticsConfig;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly PlayerDataProvider _playerDataProvider;
        private WalletPresenter _walletPresenter;
        private WinLossCountPresenter _winLossCountPresenter;

        public MainMenuScreenPresenter(MainMenuPopupService popupService, MainMenuScreenView view, WalletService walletService,
            ProjectPresentersFactory projectPresentersFactory, TypingMiniGameStatisticsService statisticsService, StatisticsConfig statisticsConfig,
            ICoroutinesPerformer coroutinesPerformer, PlayerDataProvider playerDataProvider)
        {
            _popupService = popupService;
            _view = view;
            _walletService = walletService;
            _projectPresentersFactory = projectPresentersFactory;
            _statisticsService = statisticsService;
            _statisticsConfig = statisticsConfig;
            _coroutinesPerformer = coroutinesPerformer;
            _playerDataProvider = playerDataProvider;
        }

        public void Initialize()
        {
            _view.ChooseMiniGameTypeButtonPressed += OnChooseMiniGameTypeButtonPressed;
            _view.ResetGameStatsButtonPressed += OnViewResetGameStatsButtonPressed;

            _walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_view.WalletView);
            _walletPresenter.Initialize();

            _winLossCountPresenter = _projectPresentersFactory.CreateWinLossCountPresenter(_view.WinLossCountView);
            _winLossCountPresenter.Initialize();
        }

        private void OnViewResetGameStatsButtonPressed()
        {
            int requiredCurrencyValue = _statisticsConfig.StatisticsResetPrice.MoneyValue;

            CurrencyType requiredCurrencyType = _statisticsConfig.StatisticsResetPrice.CurrencyType;

            string resetNotificationText = string.Format(ResetNotificationFormatString, requiredCurrencyValue, requiredCurrencyType);

            _popupService.ShowConfirmCancelPopup(() =>
            {
                if (_walletService.IsEnough(requiredCurrencyType, requiredCurrencyValue))
                {
                    _walletService.SpendCurrency(requiredCurrencyType, requiredCurrencyValue);
                    _statisticsService.ResetData();

                    _coroutinesPerformer.ProcessCoroutine(_playerDataProvider.Save());
                }
                else
                {
                    string failedResetPopupText = string.Format(FailedResetNotificationFormatString,
                        requiredCurrencyValue, requiredCurrencyType,
                        _walletService.GetCurrency(requiredCurrencyType).Value);

                    _popupService.ShowConfirmPopup(failedResetPopupText);
                }
            }, resetNotificationText);
        }

        private void OnChooseMiniGameTypeButtonPressed()
        {
            _popupService.ShowMiniGameTypePopup();
        }

        public void Dispose()
        {
            _view.ChooseMiniGameTypeButtonPressed -= OnChooseMiniGameTypeButtonPressed;
            _view.ResetGameStatsButtonPressed -= OnViewResetGameStatsButtonPressed;
        }
    }
}