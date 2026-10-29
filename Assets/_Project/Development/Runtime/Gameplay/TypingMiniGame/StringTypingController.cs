#region

using System;
using Runtime.Utility.Controllers;
using UnityEngine;

#endregion

namespace Runtime.Gameplay.TypingMiniGame
{
    public class StringTypingController : Controller
    {
        public event Action<string> TypedStringUpdated;

        private string _typed = "";

        protected override void ProcessUpdateInternal(float deltaTime)
        {
            bool isStringUpdated = false;

            foreach (char c in Input.inputString)
            {
                _typed += c;
                isStringUpdated = true;
            }

            if (isStringUpdated)
                TypedStringUpdated?.Invoke(_typed);
        }

        public void Reset()
        {
            _typed = "";
        }
    }
}