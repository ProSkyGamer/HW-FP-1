#region

using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Gameplay.TypingMiniGame;
using Runtime.Utility.DataSaves;
using UnityEngine;

#endregion

namespace Runtime.Utility.AssetsLoader
{
    public class ResourcesConfigLoader : IConfigsLoader
    {
        private readonly Dictionary<Type, string> _loadingAssetsPaths = new()
        {
            { typeof(TypingMiniGameConfig), "Configs/TypingMiniGameConfig" },
            { typeof(PlayerOriginDataConfig), "Configs/PlayerOriginDataConfig" },
            { typeof(StatisticsConfig), "Configs/StatisticsConfig" }
        };

        private readonly ResourcesAssetsLoader _resourcesAssetsLoader;

        public ResourcesConfigLoader(ResourcesAssetsLoader resourcesAssetsLoader)
        {
            _resourcesAssetsLoader = resourcesAssetsLoader;
        }

        public IEnumerator Load(Action<Dictionary<Type, object>> onLoadCompleted)
        {
            Dictionary<Type, object> loadedAssets = new();

            foreach (KeyValuePair<Type, string> loadingAssetsPath in _loadingAssetsPaths)
            {
                ScriptableObject asset = _resourcesAssetsLoader.Load<ScriptableObject>(loadingAssetsPath.Value);
                loadedAssets.Add(loadingAssetsPath.Key, asset);

                yield return null;
            }

            onLoadCompleted?.Invoke(loadedAssets);
        }
    }
}