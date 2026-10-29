#region

using System;

#endregion

namespace Runtime.Gameplay.TypingMiniGame
{
    public class MiniGameConditionsHandler
    {
        public event Action Won;
        public event Action Lost;

        private readonly Func<string, string, bool> _winConditions;
        private readonly Func<string, string, bool> _loseConditions;

        public MiniGameConditionsHandler(Func<string, string, bool> winConditions, Func<string, string, bool> loseConditions)
        {
            _winConditions = winConditions ?? throw new ArgumentException("Win conditions are not set!");
            _loseConditions = loseConditions ?? throw new ArgumentException("Lose conditions are not set!");
        }

        public void CheckConditions(string requiredString, string typedString)
        {
            if (_winConditions.Invoke(requiredString, typedString))
                Won?.Invoke();
            else if (_loseConditions.Invoke(requiredString, typedString))
                Lost?.Invoke();
        }
    }
}