#region

using System;
using Random = UnityEngine.Random;

#endregion

namespace Runtime.Gameplay.TypingMiniGame
{
    public class StringGenerator
    {
        private readonly string _availableSymbols;

        public StringGenerator(string availableSymbols)
        {
            if (string.IsNullOrEmpty(availableSymbols))
                throw new ArgumentNullException("availableSymbols");

            _availableSymbols = availableSymbols;
        }

        public string GenerateString(int length)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
                result[i] = _availableSymbols[Random.Range(0, _availableSymbols.Length)];

            return new string(result);
        }
    }
}