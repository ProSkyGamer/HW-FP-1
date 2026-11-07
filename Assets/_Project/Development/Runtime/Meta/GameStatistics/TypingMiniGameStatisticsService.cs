#region

using Runtime.Utility.DataSaves;
using Runtime.Utility.DataSaves.DataProviders;
using Runtime.Utility.Reacts;

#endregion

namespace Runtime.Meta.GameStatistics
{
    public class TypingMiniGameStatisticsService : IDataWriter<PlayerData>, IDataReader<PlayerData>
    {
        private readonly PlayerDataProvider _playerDataProvider;

        private readonly ReactVariable<int> _winCount = new();
        private readonly ReactVariable<int> _loseCount = new();

        public IReadonlyVariable<int> WinCount => _winCount;
        public IReadonlyVariable<int> LoseCount => _loseCount;

        public TypingMiniGameStatisticsService(PlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;

            _playerDataProvider.RegisterReader(this);
            _playerDataProvider.RegisterWriter(this);
        }

        public void WriteTo(PlayerData data)
        {
            data.WinsCount = WinCount.Value;
            data.LosesCount = LoseCount.Value;
        }

        public void ReadFrom(PlayerData data)
        {
            _winCount.Value = data.WinsCount;
            _loseCount.Value = data.LosesCount;
        }

        public void AddWin()
        {
            _winCount.Value++;
        }

        public void AddLose()
        {
            _loseCount.Value++;
        }

        public void ResetData()
        {
            _winCount.Value = 0;
            _loseCount.Value = 0;
        }
    }
}