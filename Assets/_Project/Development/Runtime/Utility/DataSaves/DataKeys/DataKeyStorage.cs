#region

using System;
using System.Collections.Generic;

#endregion

namespace Runtime.Utility.DataSaves.DataKeys
{
    public class DataKeyStorage : IDataKeysProvider
    {
        private readonly Dictionary<Type, string> _keys = new()
        {
            { typeof(PlayerData), "PlayerData" }
        };

        public string GetKeyFor<TData>() where TData : ISaveData
        {
            if (_keys.ContainsKey(typeof(TData)) == false)
                throw new NullReferenceException($"Data type {typeof(TData)} not registered in keys manager!");

            return _keys[typeof(TData)];
        }
    }
}