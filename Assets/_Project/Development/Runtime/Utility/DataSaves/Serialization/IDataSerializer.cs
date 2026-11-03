namespace Runtime.Utility.DataSaves.Serialization
{
    public interface IDataSerializer
    {
        public string Serialize<T>(T data) where T : ISaveData;
        public T Deserialize<T>(string serializedData) where T : ISaveData;
    }
}