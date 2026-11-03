#region

using System;
using System.Collections;
using System.IO;

#endregion

namespace Runtime.Utility.DataSaves.Repository
{
    public class LocalFileDataRepository : IDataRepository
    {
        private readonly string _path;
        private readonly string _fileExtension;

        public LocalFileDataRepository(string path, string fileExtension)
        {
            _path = path;
            _fileExtension = fileExtension;
        }

        public IEnumerator Write(string key, string data)
        {
            File.WriteAllText(GetPathTo(key), data);

            yield break;
        }

        public IEnumerator Read<TData>(string key, Action<string> callback)
        {
            string text = File.ReadAllText(GetPathTo(key));

            callback?.Invoke(text);

            yield break;
        }

        public IEnumerator Remove(string key)
        {
            File.Delete(GetPathTo(key));

            yield break;
        }

        public IEnumerator Exists(string key, Action<bool> callback)
        {
            bool exists = File.Exists(GetPathTo(key));

            callback?.Invoke(exists);

            yield break;
        }

        private string GetPathTo(string key)
        {
            return $"{Path.Combine(_path, key)}.{_fileExtension}";
        }
    }
}