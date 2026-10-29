#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Runtime.Utility.Controllers
{
    public class ControllersesRunner : MonoBehaviour, IControllersRunner
    {
        private readonly Dictionary<Controller, Func<bool>> _runningControllers = new();
        private readonly Dictionary<Controller, Func<bool>> _addingControllers = new();
        private readonly List<Controller> _removingControllers = new();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Register(Controller controller, Func<bool> destructionReason)
        {
            if (controller == null)
                throw new ArgumentException($"Registering controller is null");

            if (destructionReason == null)
                throw new ArgumentException($"Passing controller doesn't has a destruction reason");

            _addingControllers.Add(controller, destructionReason);
        }

        public void Remove(Controller controller)
        {
            if (_removingControllers.Contains(controller))
                return;

            if (_runningControllers.ContainsKey(controller) == false)
                return;

            _removingControllers.Remove(controller);
        }

        private void Update()
        {
            foreach (KeyValuePair<Controller, Func<bool>> controller in _addingControllers)
            {
                if (controller.Value?.Invoke() == false)
                    _runningControllers.Add(controller.Key, controller.Value);
            }

            _addingControllers.Clear();

            foreach (Controller removingController in _removingControllers)
            {
                _runningControllers.Remove(removingController);
            }

            _removingControllers.Clear();

            float deltaTime = Time.deltaTime;

            foreach (KeyValuePair<Controller, Func<bool>> controller in _runningControllers)
            {
                if (controller.Value.Invoke())
                {
                    _removingControllers.Add(controller.Key);

                    continue;
                }

                controller.Key.Update(deltaTime);
            }
        }
    }
}