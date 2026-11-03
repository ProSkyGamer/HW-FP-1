namespace Runtime.Utility.DataSaves.DataProviders
{
    public interface IDataReader<TData> where TData : ISaveData
    {
        public void ReadFrom(TData data);
    }
}