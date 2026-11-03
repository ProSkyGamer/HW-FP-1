#region

using System;
using System.Collections;
using Runtime.Utility.DataSaves.DataKeys;
using Runtime.Utility.DataSaves.Repository;
using Runtime.Utility.DataSaves.Serialization;

#endregion

namespace Runtime.Utility.DataSaves
{
    public class DataSavesService : ISaveLoadDataService
    {
        private readonly IDataKeysProvider _keysProvider;
        private readonly IDataRepository _dataRepository;
        private readonly IDataSerializer _dataSerializer;

        public DataSavesService(IDataKeysProvider keysProvider, IDataRepository dataRepository, IDataSerializer dataSerializer)
        {
            _keysProvider = keysProvider;
            _dataRepository = dataRepository;
            _dataSerializer = dataSerializer;
        }

        public IEnumerator SaveData<TData>(TData data) where TData : ISaveData
        {
            string key = _keysProvider.GetKeyFor<TData>();
            string serializedData = _dataSerializer.Serialize(data);

            yield return _dataRepository.Write(key, serializedData);
        }

        public IEnumerator ReadData<TData>(Action<TData> callback) where TData : ISaveData
        {
            string key = _keysProvider.GetKeyFor<TData>();
            string serializedData = "";

            yield return _dataRepository.Read<TData>(key, (read) => serializedData = read);

            TData data = _dataSerializer.Deserialize<TData>(serializedData);

            callback?.Invoke(data);
        }

        public IEnumerator RemoveData<TData>() where TData : ISaveData
        {
            string key = _keysProvider.GetKeyFor<TData>();

            yield return _dataRepository.Remove(key);
        }

        public IEnumerator ExistsData<TData>(Action<bool> callback) where TData : ISaveData
        {
            string key = _keysProvider.GetKeyFor<TData>();

            yield return _dataRepository.Exists(key, (result) => callback?.Invoke(result));
        }
    }
}