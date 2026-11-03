#region

using System;
using System.Text;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
using Runtime.Utility.Controllers;
using UnityEngine;

#endregion

namespace Runtime.MainMenu
{
    public class GameInfoDataProvider : IDisposable
    {
        private readonly WalletService _walletService;
        private readonly InputKeyController _inputKeyController;
        private readonly TypingMiniGameStatisticsService _statisticsService;

        public GameInfoDataProvider(WalletService walletService, TypingMiniGameStatisticsService statisticsService,
            InputKeyController inputKeyController)
        {
            _walletService = walletService;
            _inputKeyController = inputKeyController;
            _statisticsService = statisticsService;
        }

        public void Activate()
        {
            _inputKeyController.KeyCodePressed += OnKeyCodePressed;

            _inputKeyController.Activate();
        }

        private void OnKeyCodePressed(InputKeyController controller)
        {
            StringBuilder sb = new();
            sb.Append("Текущие значения кошелька\n");

            foreach (CurrencyType currencyType in _walletService.AvailableCurrencies)
            {
                sb.AppendLine($"{currencyType}: {_walletService.GetCurrency(currencyType).Value}\n");
            }

            sb.Append("Статистика игр: \n");
            sb.Append($"Выиграно: {_statisticsService.WinCount} \n");
            sb.Append($"Проиграно: {_statisticsService.LoseCount}");

            Debug.Log(sb.ToString());
        }

        public void Dispose()
        {
            _inputKeyController.KeyCodePressed -= OnKeyCodePressed;

            _inputKeyController.Deactivate();
            _inputKeyController.Destroy();
        }
    }
}