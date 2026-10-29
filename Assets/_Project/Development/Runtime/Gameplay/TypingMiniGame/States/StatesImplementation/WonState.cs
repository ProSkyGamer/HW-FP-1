#region

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

        public WonState(SceneSwitcherService sceneSwitcherService, InputKeyController inputKeyController, ICoroutinesPerformer coroutinesPerformer)
        {
            _sceneSwitcherService = sceneSwitcherService;
            _inputKeyController = inputKeyController;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public void TransitionIntoThis()
        {
            _inputKeyController.KeyCodePressed += InputKeyController_OnKeyCodePressed;
            _inputKeyController.Activate();

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