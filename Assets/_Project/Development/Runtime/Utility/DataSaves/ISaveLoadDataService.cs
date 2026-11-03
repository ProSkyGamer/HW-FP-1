#region

using System;
using System.Collections;

#endregion

namespace Runtime.Utility.DataSaves
{
    public interface ISaveLoadDataService
    {
        public IEnumerator SaveData<TData>(TData data) where TData : ISaveData;

        public IEnumerator ReadData<TData>(Action<TData> callback) where TData : ISaveData;

        public IEnumerator RemoveData<TData>() where TData : ISaveData;

        public IEnumerator ExistsData<TData>(Action<bool> callback) where TData : ISaveData;
    }
}