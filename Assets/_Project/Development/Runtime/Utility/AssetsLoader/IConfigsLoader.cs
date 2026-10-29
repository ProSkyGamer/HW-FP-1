#region

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Runtime.Utility.AssetsLoader
{
    public interface IConfigsLoader
    {
        public IEnumerator Load(Action<Dictionary<Type, object>> onLoadCompleted);
    }
}