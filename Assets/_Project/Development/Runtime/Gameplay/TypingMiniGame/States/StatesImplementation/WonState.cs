#region

using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
using Runtime.UI.Gameplay;
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
        private readonly GameplayPopupService _popupService;

        public WonState(SceneSwitcherService sceneSwitcherService, InputKeyController inputKeyController, ICoroutinesPerformer coroutinesPerformer,
            WalletService walletService, CurrencyData currencyData, TypingMiniGameStatisticsService statisticsService,
            GameplayPopupService popupService)
        {
            _sceneSwitcherService = sceneSwitcherService;
            _inputKeyController = inputKeyController;
            _coroutinesPerformer = coroutinesPerformer;
            _walletService = walletService;
            _currencyData = currencyData;
            _statisticsService = statisticsService;
            _popupService = popupService;
        }

        public void TransitionIntoThis()
        {
            _inputKeyController.KeyCodePressed += InputKeyController_OnKeyCodePressed;
            _inputKeyController.Activate();

            CurrencyType currencyType = _currencyData.CurrencyType;
            int moneyValue = _currencyData.MoneyValue;

            _popupService.ShowConfirmPopup($"Победа, начислено {moneyValue} {currencyType}! \n Нажмите, чтобы вернуться в главное меню",
                () => { _coroutinesPerformer.ProcessCoroutine(_sceneSwitcherService.SwitchSceneTo(Scenes.MainMenu)); });

            if (moneyValue > 0)
                _walletService.AddCurrency(currencyType, moneyValue);
            else if (moneyValue < 0)
                if (_walletService.IsEnough(currencyType, -moneyValue))
                    _walletService.SpendCurrency(currencyType, -moneyValue);

            _statisticsService.AddWin();

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