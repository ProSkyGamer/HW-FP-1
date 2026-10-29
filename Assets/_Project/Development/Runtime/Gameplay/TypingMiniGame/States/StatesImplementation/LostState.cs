#region

using Runtime.Gameplay.Bootstrap;
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

        public LostState(SceneSwitcherService sceneSwitcherService, InputKeyController inputKeyController, ICoroutinesPerformer coroutinesPerformer,
            GameplaySceneArgs gameplaySceneArgs)
        {
            _sceneSwitcherService = sceneSwitcherService;
            _inputKeyController = inputKeyController;
            _coroutinesPerformer = coroutinesPerformer;

            _gameplaySceneArgs = gameplaySceneArgs; // если честно вообще не нравится эта строка
            // лучше наверное было бы чисто переходить назад в игровой стейт,
            // но там в задании написано спецефически перезгразиться сцену, поэтому что есть
        }

        public void TransitionIntoThis()
        {
            _inputKeyController.KeyCodePressed += InputKeyController_OnKeyCodePressed;
            _inputKeyController.Activate();

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