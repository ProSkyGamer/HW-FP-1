#region

using System.Collections.Generic;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.Utility.AssetsLoader;

#endregion

namespace Runtime.Utility.DataSaves.DataProviders
{
    public class PlayerDataProvider : DataProvider<PlayerData>
    {
        private readonly GameConfigsProvider _configsProvider;

        public PlayerDataProvider(ISaveLoadDataService saveLoadDataService, GameConfigsProvider configsProvider) : base(saveLoadDataService)
        {
            _configsProvider = configsProvider;
        }

        protected override PlayerData GetOriginData()
        {
            PlayerData data = new()
            {
                WinsCount = 0,
                LosesCount = 0,
                WalletData = InitWalletData()
            };

            return data;
        }

        private Dictionary<CurrencyType, int> InitWalletData()
        {
            Dictionary<CurrencyType, int> walletData = new();
            PlayerOriginDataConfig playerOriginDataConfig = _configsProvider.GetConfig<PlayerOriginDataConfig>();

            foreach (CurrencyType currencyType in playerOriginDataConfig.GetAvailableCurrencies())
            {
                walletData[currencyType] = playerOriginDataConfig.GetCurrencyValue(currencyType);
            }

            return walletData;
        }
    }
}