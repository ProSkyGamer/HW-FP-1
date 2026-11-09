#region

using Runtime.Gameplay.Bootstrap;
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
    public class LostState : IMiniGameStateHandler
    {
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly GameplaySceneArgs _gameplaySceneArgs;
        private readonly InputKeyController _inputKeyController;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly WalletService _walletService;
        private readonly CurrencyData _currencyData;
        private readonly TypingMiniGameStatisticsService _statisticsService;
        private readonly GameplayPopupService _popupService;

        public LostState(SceneSwitcherService sceneSwitcherService, InputKeyController inputKeyController, ICoroutinesPerformer coroutinesPerformer,
            GameplaySceneArgs gameplaySceneArgs, WalletService walletService, CurrencyData currencyData,
            TypingMiniGameStatisticsService statisticsService, GameplayPopupService popupService)
        {
            _sceneSwitcherService = sceneSwitcherService;
            _inputKeyController = inputKeyController;
            _coroutinesPerformer = coroutinesPerformer;
            _walletService = walletService;
            _currencyData = currencyData;
            _statisticsService = statisticsService;
            _popupService = popupService;

            _gameplaySceneArgs = gameplaySceneArgs;
        }

        public void TransitionIntoThis()
        {
            _inputKeyController.KeyCodePressed += InputKeyController_OnKeyCodePressed;
            _inputKeyController.Activate();

            CurrencyType currencyType = _currencyData.CurrencyType;
            int moneyValue = _currencyData.MoneyValue;

            _popupService.ShowConfirmPopup($"Поражений, вычтено {moneyValue} {currencyType}! \n Нажмите, чтобы повторить попытку",
                () => { _coroutinesPerformer.ProcessCoroutine(_sceneSwitcherService.SwitchSceneTo(Scenes.Gameplay, _gameplaySceneArgs)); });
            // TODO
            // прям совсем неправильно чувствуется, делать это здесь, но с другой стороны, а если мне нужно тупо подождать пока
            // закроется уведомление, а потом сделать какую-то логику (что такое callback, кто бы о нем помнил)
            // писать что ли обертку какую-то чисто под стейты и выкидывать ивент о смене стейта?
            // короче я хз, ничего из этого не звучит нормально, поэтому тут оставлю просто

            if (moneyValue > 0)
                _walletService.AddCurrency(currencyType, moneyValue);
            else if (moneyValue < 0)
                if (_walletService.IsEnough(currencyType, -moneyValue))
                    _walletService.SpendCurrency(currencyType, -moneyValue);

            _statisticsService.AddLose();

            Debug.Log($"GAME LOST! Press {_inputKeyController.KeyCode} to restart!");
        }

        private void InputKeyController_OnKeyCodePressed(InputKeyController obj)
        {
            _coroutinesPerformer.ProcessCoroutine(_sceneSwitcherService.SwitchSceneTo(Scenes.Gameplay, _gameplaySceneArgs));
        }

        public void TransitionOutOfThis()
        {
            _inputKeyController.KeyCodePressed -= InputKeyController_OnKeyCodePressed;
            _inputKeyController.Deactivate();
            _inputKeyController.Destroy();
        }
    }
}