#region

using System.Collections.Generic;
using Runtime.Gameplay.Meta.Wallet;

#endregion

namespace Runtime.Utility.DataSaves
{
    public class PlayerData : ISaveData
    {
        public Dictionary<CurrencyType, int> WalletData;

        public int WinsCount;
        public int LosesCount;
    }
}