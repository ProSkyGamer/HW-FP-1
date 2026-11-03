#region

using System;
using UnityEngine;

#endregion

namespace Runtime.Gameplay.Meta.Wallet
{
    [Serializable]
    public class CurrencyData
    {
        [field: SerializeField] public CurrencyType CurrencyType { get; private set; }
        [field: SerializeField] public int MoneyValue { get; private set; }
    }
}