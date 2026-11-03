#region

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Runtime.Utility.DataSaves.DataProviders
{
    public abstract class DataProvider<TData> where TData : ISaveData
    {
        private readonly ISaveLoadDataService _saveLoadDataService;
        private TData _data;

        private readonly List<IDataWriter<TData>> _writers = new();
        private readonly List<IDataReader<TData>> _readers = new();

        protected DataProvider(ISaveLoadDataService saveLoadDataService)
        {
            _saveLoadDataService = saveLoadDataService;
            _data = default;
        }

        public void RegisterReader(IDataReader<TData> reader)
        {
            _readers.Add(reader);
        }

        public void RegisterWriter(IDataWriter<TData> writer)
        {
            _writers.Add(writer);
        }

        public IEnumerator Load()
        {
            yield return _saveLoadDataService.ReadData<TData>(loaded => _data = loaded);

            SendDataToReaders();
        }

        private void SendDataToReaders()
        {
            foreach (IDataReader<TData> reader in _readers)
            {
                reader.ReadFrom(_data);
            }
        }

        public IEnumerator Save()
        {
            WriteDataFromWriters();

            yield return _saveLoadDataService.SaveData(_data);
        }

        private void WriteDataFromWriters()
        {
            foreach (IDataWriter<TData> writer in _writers)
            {
                writer.WriteTo(_data);
            }
        }

        public IEnumerator Exists(Action<bool> callback)
        {
            yield return _saveLoadDataService.ExistsData<TData>(callback);
        }

        public void Reset()
        {
            _data = GetOriginData();

            SendDataToReaders();
        }

        protected abstract TData GetOriginData();
    }
}