#region

using System.Collections;
using UnityEngine;

#endregion

namespace Runtime.Utility.Coroutines
{
    public interface ICoroutinesPerformer
    {
        public Coroutine ProcessCoroutine(IEnumerator coroutine);

        public void StopProcessingCoroutine(Coroutine coroutine);
    }
}