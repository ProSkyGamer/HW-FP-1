#region

using Runtime.Gameplay.Bootstrap;
using Runtime.UI.Core;
using Runtime.Utility.Coroutines;
using Runtime.Utility.SceneManagement;

#endregion

namespace Runtime.UI.MiniGameTypePopup
{
    public class MiniGameTypeTilePresenter : IPresenter
    {
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly MiniGameTypeTileView _view;
        private readonly MiniGameType _miniGameType;

        public MiniGameTypeTilePresenter(SceneSwitcherService sceneSwitcher, ICoroutinesPerformer coroutinesPerformer, MiniGameTypeTileView view,
            MiniGameType miniGameType)
        {
            _sceneSwitcher = sceneSwitcher;
            _coroutinesPerformer = coroutinesPerformer;
            _view = view;
            _miniGameType = miniGameType;
        }

        public void Initialize()
        {
            _view.Clicked += OnViewClicked;
            _view.SetText(_miniGameType.ToString());
        }

        private void OnViewClicked()
        {
            _coroutinesPerformer.ProcessCoroutine(_sceneSwitcher.SwitchSceneTo(Scenes.Gameplay, new GameplaySceneArgs(_miniGameType)));
        }

        public void Dispose()
        {
            _view.Clicked -= OnViewClicked;
        }
    }
}