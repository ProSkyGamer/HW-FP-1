#region

using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.SceneManagement;
using UnityEngine;

#endregion

namespace Runtime.Gameplay.TypingMiniGame.States.StatesImplementation
{
    public class WonState : IMiniGameStateHandler
    {
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly InputKeyController _inputKeyController;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly WalletService _walletService;
        private readonly CurrencyData _currencyData;
        private readonly TypingMiniGameStatisticsService _statisticsService;

        public WonState(SceneSwitcherService sceneSwitcherService, InputKeyController inputKeyController, ICoroutinesPerformer coroutinesPerformer,
            WalletService walletService, CurrencyData currencyData, TypingMiniGameStatisticsService statisticsService)
        {
            _sceneSwitcherService = sceneSwitcherService;
            _inputKeyController = inputKeyController;
            _coroutinesPerformer = coroutinesPerformer;
            _walletService = walletService;
            _currencyData = currencyData;
            _statisticsService = statisticsService;
        }

        public void TransitionIntoThis()
        {
            _inputKeyController.KeyCodePressed += InputKeyController_OnKeyCodePressed;
            _inputKeyController.Activate();

            if (_currencyData.MoneyValue > 0)
                _walletService.AddCurrency(_currencyData.CurrencyType, _currencyData.MoneyValue);
            else if (_currencyData.MoneyValue < 0)
                if (_walletService.IsEnough(_currencyData.CurrencyType, -_currencyData.MoneyValue))
                    _walletService.SpendCurrency(_currencyData.CurrencyType, -_currencyData.MoneyValue);

            Debug.Log($"Stats {_statisticsService.WinCount}");
            _statisticsService.AddWin();
            Debug.Log($"Stats {_statisticsService.WinCount}");

            Debug.Log($"GAME WON! Press {_inputKeyController.KeyCode} to go to main menu!");
        }

        private void InputKeyController_OnKeyCodePressed(InputKeyController obj)
        {
            _coroutinesPerformer.ProcessCoroutine(_sceneSwitcherService.SwitchSceneTo(Scenes.MainMenu));
        }

        public void TransitionOutOfThis()
        {
            _inputKeyController.KeyCodePressed -= InputKeyController_OnKeyCodePressed;
            _inputKeyController.Deactivate();
            _inputKeyController.Destroy();
        }
    }
}