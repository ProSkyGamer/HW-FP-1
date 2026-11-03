#region

using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Gameplay.Bootstrap;
using Runtime.Gameplay.Meta.Wallet;
using UnityEngine;

#endregion

namespace Runtime.Gameplay.TypingMiniGame
{
    [CreateAssetMenu(fileName = "TypingMiniGameConfig", menuName = "Gameplay/Config/TypingMiniGameConfig", order = 0)]
    public class TypingMiniGameConfig : ScriptableObject
    {
        [Serializable]
        private class MiniGameConfig
        {
            [field: SerializeField] public MiniGameType MiniGameType { get; private set; }
            [field: SerializeField] public string Symbols { get; private set; }
            [field: SerializeField] public CurrencyData DeltaCurrencyOnWin { get; private set; }
            [field: SerializeField] public CurrencyData DeltaCurrencyOnLose { get; private set; }
        }

        [SerializeField] private List<MiniGameConfig> _miniGameAvailableSymbols;

        [field: SerializeField] public int GeneratedStringLength { get; private set; }

        public List<MiniGameType> GetConfiguredMiniGameTypes()
        {
            return _miniGameAvailableSymbols.Select(miniGameConfig => miniGameConfig.MiniGameType).ToList();
        }

        public string GetMiniGameSymbols(MiniGameType type)
        {
            string foundSymbols = _miniGameAvailableSymbols.FirstOrDefault(miniGameConfig => miniGameConfig.MiniGameType == type)?.Symbols ??
                                  throw new NullReferenceException($"No game config for {type}");

            return foundSymbols;
        }

        public CurrencyData GetDeltaCurrencyOnWin(MiniGameType type)
        {
            CurrencyData deltaCurrency =
                _miniGameAvailableSymbols.FirstOrDefault(miniGameConfig => miniGameConfig.MiniGameType == type)?.DeltaCurrencyOnWin ??
                throw new NullReferenceException($"No game config for {type}");

            return deltaCurrency;
        }

        public CurrencyData GetDeltaCurrencyOnLose(MiniGameType type)
        {
            CurrencyData deltaCurrency =
                _miniGameAvailableSymbols.FirstOrDefault(miniGameConfig => miniGameConfig.MiniGameType == type)?.DeltaCurrencyOnLose ??
                throw new NullReferenceException($"No game config for {type}");

            return deltaCurrency;
        }
    }
}