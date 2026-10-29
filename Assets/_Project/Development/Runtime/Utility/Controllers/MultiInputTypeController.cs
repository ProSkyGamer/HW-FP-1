#region

using System;
using System.Collections.Generic;

#endregion

namespace Runtime.Utility.Controllers
{
    public class MultiInputTypeController<T> : Controller, IDisposable
    {
        public event Action<T> TypeChosen;

        private readonly Dictionary<InputKeyController, T> _inputDictionary = new();

        public void RegisterInputController(T type, InputKeyController inputKeyController)
        {
            _inputDictionary.Add(inputKeyController, type);

            inputKeyController.KeyCodePressed += InputKeyController_OnKeyCodePressed;

            inputKeyController.Activate();
        }

        private void InputKeyController_OnKeyCodePressed(InputKeyController triggeredController)
        {
            if (_inputDictionary.TryGetValue(triggeredController, out T value) == false)
                return;

            TypeChosen?.Invoke(value);
        }

        protected override void ProcessUpdateInternal(float deltaTime)
        {
            foreach (InputKeyController inputKeyController in _inputDictionary.Keys)
            {
                inputKeyController.Update(deltaTime);
            }
        }

        public void Dispose()
        {
            foreach (InputKeyController inputKeyController in _inputDictionary.Keys)
            {
                inputKeyController.KeyCodePressed -= InputKeyController_OnKeyCodePressed;
                inputKeyController.Deactivate();
                inputKeyController.Destroy();
            }

            _inputDictionary.Clear();
        }
    }
}