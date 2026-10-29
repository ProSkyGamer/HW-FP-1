#region

using System.Collections;
using Runtime.Utility.DI;
using UnityEngine;

#endregion

namespace Runtime.Utility.Bootstrap
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        public abstract void HandleSceneRegistrations(DIContainer container, ISceneArgs sceneArgs);

        public abstract IEnumerator Initialize();

        public abstract void StartScene();

        public abstract IEnumerator UnloadScene();
    }
}