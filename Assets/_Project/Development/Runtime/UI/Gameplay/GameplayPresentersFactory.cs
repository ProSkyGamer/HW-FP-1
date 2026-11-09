#region

using Runtime.Gameplay.TypingMiniGame;
using Runtime.Utility.Coroutines;
using Runtime.Utility.DI;
using Runtime.Utility.SceneManagement;

#endregion

namespace Runtime.UI.Gameplay
{
    public class GameplayPresentersFactory
    {
        private readonly DIContainer _container;

        public GameplayPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public GameplayScreenPresenter CreateGameplayScreenPresenter(GameplayScreenView view)
        {
            return new GameplayScreenPresenter(view,
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<TypingMiniGameHandler>(),
                _container.Resolve<ICoroutinesPerformer>());
        }
    }
}