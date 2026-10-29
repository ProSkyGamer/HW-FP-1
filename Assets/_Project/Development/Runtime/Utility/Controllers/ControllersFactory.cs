#region

using System.Collections.Generic;
using Runtime.Gameplay.TypingMiniGame;
using Runtime.Gameplay.TypingMiniGame.States;
using Runtime.Gameplay.TypingMiniGame.States.StateTransitions;
using UnityEngine;

#endregion

namespace Runtime.Utility.Controllers
{
    public class ControllersFactory
    {
        private readonly IControllersRunner _controllersRunner;

        public ControllersFactory(IControllersRunner controllersRunner)
        {
            _controllersRunner = controllersRunner;
        }

        public InputKeyController CreateInputKeyController(KeyCode keyCode)
        {
            InputKeyController inputController = new InputKeyController(keyCode);

            _controllersRunner.Register(inputController, () => inputController.IsNeedToBeDestroyed);

            return inputController;
        }

        public MultiInputTypeController<T> CreateMultiInputTypeController<T>(Dictionary<T, KeyCode> typeToKeyCodeDict)
        {
            MultiInputTypeController<T> multiInputTypeController = new MultiInputTypeController<T>();

            foreach (KeyValuePair<T, KeyCode> keyCode in typeToKeyCodeDict)
            {
                InputKeyController inputController = new InputKeyController(keyCode.Value);

                multiInputTypeController.RegisterInputController(keyCode.Key, inputController);
            }

            _controllersRunner.Register(multiInputTypeController, () => multiInputTypeController.IsNeedToBeDestroyed);

            return multiInputTypeController;
        }

        public StringTypingController CreateStringTypingController()
        {
            StringTypingController stringTypingController = new StringTypingController();

            _controllersRunner.Register(stringTypingController, () => stringTypingController.IsNeedToBeDestroyed);

            return stringTypingController;
        }

        public TypingMiniGameStateController CreateTypingMiniGameStateController(
            Dictionary<TypingMiniGameState, IMiniGameStateHandler> miniGameStateHandlers,
            List<IStateTransitionCondition> stateTransitionConditions, TypingMiniGameState startingState)
        {
            TypingMiniGameStateController typingMiniGameStateController = new(miniGameStateHandlers, stateTransitionConditions, startingState);

            _controllersRunner.Register(typingMiniGameStateController, () => typingMiniGameStateController.IsNeedToBeDestroyed);

            typingMiniGameStateController.Initialize();

            return typingMiniGameStateController;
        }
    }
}