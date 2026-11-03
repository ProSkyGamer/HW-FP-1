namespace Runtime.Utility.DataSaves.DataKeys
{
    public interface IDataKeysProvider
    {
        public string GetKeyFor<TData>() where TData : ISaveData;
    }
}