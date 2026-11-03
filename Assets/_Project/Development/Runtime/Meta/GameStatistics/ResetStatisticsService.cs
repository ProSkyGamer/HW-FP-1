#region

using System;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DataSaves.DataProviders;
using UnityEngine;

#endregion

namespace Runtime.Meta.GameStatistics
{
    public class ResetStatisticsService : IDisposable
    {
        private readonly StatisticsConfig _config;
        private readonly WalletService _walletService;
        private readonly TypingMiniGameStatisticsService _statisticsService;
        private readonly InputKeyController _inputKeyController;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly PlayerDataProvider _playerDataProvider;

        public ResetStatisticsService(StatisticsConfig config, WalletService walletService, TypingMiniGameStatisticsService statisticsService,
            InputKeyController inputKeyController, ICoroutinesPerformer coroutinesPerformer, PlayerDataProvider playerDataProvider)
        {
            _config = config;
            _walletService = walletService;
            _statisticsService = statisticsService;
            _inputKeyController = inputKeyController;
            _coroutinesPerformer = coroutinesPerformer;
            _playerDataProvider = playerDataProvider;
        }

        public void Activate()
        {
            _inputKeyController.KeyCodePressed += OnKeyCodePressed;

            _inputKeyController.Activate();
        }

        private void OnKeyCodePressed(InputKeyController obj)
        {
            CurrencyType requiredCurrencyType = _config.StatisticsResetPrice.CurrencyType;
            int requiredCurrencyValue = _config.StatisticsResetPrice.MoneyValue;

            if (_walletService.IsEnough(requiredCurrencyType, requiredCurrencyValue) == false)
            {
                Debug.Log(
                    $"Not enough currency {requiredCurrencyType}! " +
                    $"Required {requiredCurrencyValue} while possessing {_walletService.GetCurrency(requiredCurrencyType).Value}");
            }
            else
            {
                _walletService.SpendCurrency(requiredCurrencyType, requiredCurrencyValue);
                _statisticsService.ResetData();
                _coroutinesPerformer.ProcessCoroutine(_playerDataProvider.Save());

                Debug.Log("Win/Lose count info reset!");
            }
        }

        public void Dispose()
        {
            _inputKeyController.KeyCodePressed -= OnKeyCodePressed;

            _inputKeyController.Deactivate();
            _inputKeyController.Destroy();
        }
    }
}