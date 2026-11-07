#region

using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Gameplay.Meta.Wallet;
using UnityEngine;

#endregion

[CreateAssetMenu(fileName = "CurrencyIconConfig", menuName = "Config/Wallet/CurrencyIconConfig", order = 0)]
public class CurrencyIconConfig : ScriptableObject
{
    [Serializable]
    private class CurrencyIconConfigData
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public CurrencyType Currency { get; private set; }
    }

    [SerializeField] private List<CurrencyIconConfigData> _data = new();

    public Sprite GetIconFor(CurrencyType currency)
    {
        return _data.First(configData => configData.Currency == currency).Icon ??
               throw new NullReferenceException($"No icon found for currency {currency}");
    }
}