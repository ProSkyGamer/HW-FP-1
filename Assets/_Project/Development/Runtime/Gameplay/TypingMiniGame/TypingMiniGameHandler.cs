#region

using System;
using System.Collections.Generic;
using Runtime.Gameplay.Bootstrap;
using UnityEngine;

#endregion

namespace Runtime.Gameplay.TypingMiniGame
{
    public class TypingMiniGameHandler
    {
        private MiniGameType _miniGameType;

        private readonly Dictionary<MiniGameType, StringGenerator> _miniGameTypeToStringGenerator;
        private readonly MiniGameConditionsHandler _miniGameConditionsHandler;
        private readonly int _requiredStringLength;

        private readonly StringTypingController _stringTypingController;
        private string _requiredString;

        public TypingMiniGameHandler(Dictionary<MiniGameType, StringGenerator> miniGameTypeToStringGenerator,
            MiniGameConditionsHandler miniGameConditionsHandler, int requiredStringLength,
            StringTypingController stringTypingController)
        {
            _miniGameTypeToStringGenerator = new Dictionary<MiniGameType, StringGenerator>(miniGameTypeToStringGenerator);
            _miniGameConditionsHandler = miniGameConditionsHandler;
            _requiredStringLength = requiredStringLength;
            _stringTypingController = stringTypingController;
        }

        public void SetGameType(MiniGameType miniGameType)
        {
            _miniGameType = miniGameType;
        }

        public void StartMiniGame()
        {
            if (_miniGameTypeToStringGenerator.TryGetValue(_miniGameType, out StringGenerator stringGenerator) == false)
                throw new NullReferenceException($"No string generator for {_miniGameType} mini game type");

            _requiredString = stringGenerator.GenerateString(_requiredStringLength);

            _stringTypingController.TypedStringUpdated += StringTypingController_OnTypedStringUpdated;

            _stringTypingController.Reset();
            _stringTypingController.Activate();

            Debug.Log($"Game started! Type : {_miniGameType}!");
            Debug.Log($"Required sequence to type {_requiredString}!");
        }

        private void StringTypingController_OnTypedStringUpdated(string typedString)
        {
            Debug.Log($"Currently typed: {typedString}, required to type: {_requiredString}");

            _miniGameConditionsHandler.CheckConditions(_requiredString, typedString);
        }

        public void EndMiniGame()
        {
            _stringTypingController.Deactivate();
        }
    }
}