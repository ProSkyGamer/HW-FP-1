#region

using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Utility.DataSaves;
using Runtime.Utility.DataSaves.DataProviders;
using Runtime.Utility.Reacts;

#endregion

namespace Runtime.Gameplay.Meta.Wallet
{
    public class WalletService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private readonly Dictionary<CurrencyType, ReactVariable<int>> _currencies;

        public IReadOnlyList<CurrencyType> AvailableCurrencies => _currencies.Keys.ToList();

        public WalletService(Dictionary<CurrencyType, ReactVariable<int>> currencies, PlayerDataProvider playerDataProvider)
        {
            _currencies = new Dictionary<CurrencyType, ReactVariable<int>>(currencies);

            playerDataProvider.RegisterReader(this);
            playerDataProvider.RegisterWriter(this);
        }

        public void ReadFrom(PlayerData data)
        {
            foreach (KeyValuePair<CurrencyType, int> typeToValue in data.WalletData)
            {
                if (_currencies.ContainsKey(typeToValue.Key))
                    _currencies[typeToValue.Key].Value = typeToValue.Value;
                else
                    _currencies.Add(typeToValue.Key, new ReactVariable<int>(typeToValue.Value));
            }
        }

        public void WriteTo(PlayerData data)
        {
            foreach (KeyValuePair<CurrencyType, ReactVariable<int>> typeToValue in _currencies)
            {
                data.WalletData[typeToValue.Key] = typeToValue.Value.Value;
            }
        }

        public void AddCurrency(CurrencyType type, int value)
        {
            if (_currencies.ContainsKey(type) == false)
                _currencies.Add(type, new ReactVariable<int>(value));
            else
                _currencies[type].Value += value;
        }

        public bool IsEnough(CurrencyType type, int value)
        {
            if (_currencies.ContainsKey(type) == false)
                return false;

            return _currencies[type].Value >= value;
        }

        public void SpendCurrency(CurrencyType type, int value)
        {
            if (IsEnough(type, value) == false)
                throw new ArgumentOutOfRangeException($"Trying to spend {value} of {type} while posessing less");

            _currencies[type].Value -= value;
        }

        public IReadonlyVariable<int> GetCurrency(CurrencyType type)
        {
            if (_currencies.ContainsKey(type) == false)
                throw new ArgumentException($"Wallet does not store {type}");

            return _currencies[type];
        }
    }
}