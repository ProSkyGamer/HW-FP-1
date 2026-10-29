#region

using System;
using Runtime.Gameplay.Bootstrap;
using Runtime.Utility.Controllers;
using Runtime.Utility.Coroutines;
using Runtime.Utility.SceneManagement;

#endregion

namespace Runtime.MainMenu.Controllers
{
    public class MiniGameChooseService : IDisposable
    {
        private readonly MultiInputTypeController<MiniGameType> _miniGameTypeInputController;
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        public MiniGameChooseService(MultiInputTypeController<MiniGameType> miniGameTypeInputController, SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer)
        {
            _miniGameTypeInputController = miniGameTypeInputController;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public void Activate()
        {
            _miniGameTypeInputController.TypeChosen += MiniGameTypeInputController_OnTypeChosen;

            _miniGameTypeInputController.Activate();
        }

        private void MiniGameTypeInputController_OnTypeChosen(MiniGameType minigameType)
        {
            StartMiniGameBy(minigameType);
        }

        private void StartMiniGameBy(MiniGameType minigameType)
        {
            GameplaySceneArgs gameplaySceneArgs = new(minigameType);
            _coroutinesPerformer.ProcessCoroutine(_sceneSwitcherService.SwitchSceneTo(Scenes.Gameplay, gameplaySceneArgs));
        }

        public void Dispose()
        {
            _miniGameTypeInputController.TypeChosen -= MiniGameTypeInputController_OnTypeChosen;

            _miniGameTypeInputController.Deactivate();
            _miniGameTypeInputController.Destroy();
        }
    }
}