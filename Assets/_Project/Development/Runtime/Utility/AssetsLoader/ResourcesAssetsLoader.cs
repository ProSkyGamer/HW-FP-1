#region

using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace Runtime.Utility.AssetsLoader
{
    public class ResourcesAssetsLoader
    {
        public T Load<T>([PathReference("~/Assets/_Project/Resources")] string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}