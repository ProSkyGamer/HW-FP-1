#region

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

#endregion

namespace Runtime.Utility.AssetsLoader
{
    public class GameConfigsLoader
    {
        private readonly IConfigsLoader[] _configsLoaders;

        private readonly Dictionary<Type, object> _configs = new();

        public GameConfigsLoader(params IConfigsLoader[] configsLoaders)
        {
            _configsLoaders = configsLoaders;
        }

        public IEnumerator LoadAll()
        {
            _configs.Clear();

            foreach (IConfigsLoader configsLoader in _configsLoaders)
            {
                yield return configsLoader.Load(configs => _configs.AddRange(configs));
            }
        }

        public T GetConfig<T>() where T : class
        {
            if (_configs.ContainsKey(typeof(T)) == false)
                throw new NullReferenceException($"Config of type {typeof(T)} never registered");

            return (T)_configs[typeof(T)];
        }
    }
}