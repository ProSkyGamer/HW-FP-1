#region

using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Gameplay.Meta.Wallet;
using UnityEngine;

#endregion

namespace Runtime.Utility.DataSaves
{
    [CreateAssetMenu(fileName = "PlayerOriginDataConfig", menuName = "Config/PlayerData/PlayerOriginDataConfig", order = 0)]
    public class PlayerOriginDataConfig : ScriptableObject
    {
        [Serializable]
        public class CurrencyStartData
        {
            [field: SerializeField] public CurrencyType CurrencyType { get; private set; }
            [field: SerializeField] public int Value { get; private set; }
        }

        [field: SerializeField] public List<CurrencyStartData> StartingWallet { get; private set; }

        public IReadOnlyList<CurrencyType> GetAvailableCurrencies()
        {
            return StartingWallet.Select(value => value.CurrencyType).ToList();
        }

        public int GetCurrencyValue(CurrencyType type)
        {
            return StartingWallet.First(value => value.CurrencyType == type).Value;
        }
    }
}