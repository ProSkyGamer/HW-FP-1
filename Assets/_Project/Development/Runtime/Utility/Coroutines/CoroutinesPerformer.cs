#region

using System.Collections;
using UnityEngine;

#endregion

namespace Runtime.Utility.Coroutines
{
    public class CoroutinesPerformer : MonoBehaviour, ICoroutinesPerformer
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public Coroutine ProcessCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void StopProcessingCoroutine(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }
    }
}