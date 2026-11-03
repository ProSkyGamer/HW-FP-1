namespace Runtime.Utility.DataSaves.DataProviders
{
    public interface IDataWriter<TData> where TData : ISaveData
    {
        public void WriteTo(TData data);
    }
}