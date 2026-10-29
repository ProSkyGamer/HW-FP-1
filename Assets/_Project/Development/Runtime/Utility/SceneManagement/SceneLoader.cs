#region

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace Runtime.Utility.SceneManagement
{
    public class SceneLoader
    {
        public IEnumerator LoadAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            AsyncOperation loader = SceneManager.LoadSceneAsync(sceneName, mode);

            if (loader == null)
                throw new NullReferenceException($"Scene with name {sceneName} could not be loaded");

            if (loader.isDone)
                yield break;

            yield return new WaitWhile(() => loader is { isDone: false });
        }

        public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneName, mode);
        }

        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}