#region

using Newtonsoft.Json;

#endregion

namespace Runtime.Utility.DataSaves.Serialization
{
    public class JsonDataSerializer : IDataSerializer
    {
        public string Serialize<T>(T data) where T : ISaveData
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public T Deserialize<T>(string serializedData) where T : ISaveData
        {
            return JsonConvert.DeserializeObject<T>(serializedData, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}