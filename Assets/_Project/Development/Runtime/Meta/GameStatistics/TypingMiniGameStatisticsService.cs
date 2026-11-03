#region

using Runtime.Utility.DataSaves;
using Runtime.Utility.DataSaves.DataProviders;

#endregion

namespace Runtime.Meta.GameStatistics
{
    public class TypingMiniGameStatisticsService : IDataWriter<PlayerData>, IDataReader<PlayerData>
    {
        private readonly PlayerDataProvider _playerDataProvider;

        public int WinCount { get; private set; }
        public int LoseCount { get; private set; }

        public TypingMiniGameStatisticsService(PlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;

            _playerDataProvider.RegisterReader(this);
            _playerDataProvider.RegisterWriter(this);
        }

        public void WriteTo(PlayerData data)
        {
            data.WinsCount = WinCount;
            data.LosesCount = LoseCount;
        }

        public void ReadFrom(PlayerData data)
        {
            WinCount = data.WinsCount;
            LoseCount = data.LosesCount;
        }

        public void AddWin()
        {
            WinCount++;
        }

        public void AddLose()
        {
            LoseCount++;
        }

        public void ResetData()
        {
            WinCount = 0;
            LoseCount = 0;
        }
    }
}