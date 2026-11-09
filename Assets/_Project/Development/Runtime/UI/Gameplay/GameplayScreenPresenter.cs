#region

using Runtime.Gameplay.TypingMiniGame;
using Runtime.UI.Core;
using Runtime.Utility.Coroutines;
using Runtime.Utility.SceneManagement;

#endregion

namespace Runtime.UI.Gameplay
{
    public class GameplayScreenPresenter : IPresenter
    {
        private const string DefaultRequiredText = "Тут будет отображаться запрашиваемый текст";
        private const string DefaultTypedText = "Тут будет отображаться введенный текст";

        private readonly GameplayScreenView _view;
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly TypingMiniGameHandler _typingMiniGameHandler;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        public GameplayScreenPresenter(GameplayScreenView view, SceneSwitcherService sceneSwitcher, TypingMiniGameHandler typingMiniGameHandler,
            ICoroutinesPerformer coroutinesPerformer)
        {
            _view = view;
            _sceneSwitcher = sceneSwitcher;
            _typingMiniGameHandler = typingMiniGameHandler;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public void Initialize()
        {
            _view.BackToMenuClicked += OnBackToMenuClicked;

            _typingMiniGameHandler.RequiredStringUpdated += OnRequiredStringUpdated;
            _typingMiniGameHandler.TypingStringUpdated += OnTypingStringUpdated;

            _view.SetRequiredText(DefaultRequiredText);
            _view.SetCurrentText(DefaultTypedText);
        }

        private void OnTypingStringUpdated(string currentText)
        {
            _view.SetCurrentText(currentText);
        }

        private void OnRequiredStringUpdated(string requiredText)
        {
            _view.SetRequiredText(requiredText);
        }

        private void OnBackToMenuClicked()
        {
            _coroutinesPerformer.ProcessCoroutine(_sceneSwitcher.SwitchSceneTo(Scenes.MainMenu));
        }

        public void Dispose()
        {
            _view.BackToMenuClicked -= OnBackToMenuClicked;

            _typingMiniGameHandler.RequiredStringUpdated -= OnRequiredStringUpdated;
            _typingMiniGameHandler.TypingStringUpdated -= OnTypingStringUpdated;
        }
    }
}