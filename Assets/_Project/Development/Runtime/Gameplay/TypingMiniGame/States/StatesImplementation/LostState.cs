#region

using Runtime.Gameplay.Bootstrap;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.Meta.GameStatistics;
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

        public LostState(SceneSwitcherService sceneSwitcherService, InputKeyController inputKeyController, ICoroutinesPerformer coroutinesPerformer,
            GameplaySceneArgs gameplaySceneArgs, WalletService walletService, CurrencyData currencyData,
            TypingMiniGameStatisticsService statisticsService)
        {
            _sceneSwitcherService = sceneSwitcherService;
            _inputKeyController = inputKeyController;
            _coroutinesPerformer = coroutinesPerformer;
            _walletService = walletService;
            _currencyData = currencyData;
            _statisticsService = statisticsService;

            _gameplaySceneArgs = gameplaySceneArgs; // если честно вообще не нравится эта строка
            // лучше наверное было бы чисто переходить назад в игровой стейт,
            // но там в задании написано спецефически перезгразиться сцену, поэтому что есть
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