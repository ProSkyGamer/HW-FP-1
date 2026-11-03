#region

using System;
using System.Collections;

#endregion

namespace Runtime.Utility.DataSaves.Repository
{
    public interface IDataRepository
    {
        public IEnumerator Write(string key, string data);
        public IEnumerator Read<TData>(string key, Action<string> callback);
        public IEnumerator Remove(string key);
        public IEnumerator Exists(string key, Action<bool> callback);
    }
}