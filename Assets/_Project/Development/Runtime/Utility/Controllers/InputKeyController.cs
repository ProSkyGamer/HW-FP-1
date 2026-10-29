#region

using System;
using UnityEngine;

#endregion

namespace Runtime.Utility.Controllers
{
    public class InputKeyController : Controller
    {
        public event Action<InputKeyController> KeyCodePressed;

        public KeyCode KeyCode { get; }

        public InputKeyController(KeyCode listeningKeyCode)
        {
            KeyCode = listeningKeyCode;
        }

        protected override void ProcessUpdateInternal(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode))
                KeyCodePressed?.Invoke(this);
        }
    }
}