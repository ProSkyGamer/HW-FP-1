#region

using System;

#endregion

namespace Runtime.Utility.Reacts
{
    public class Subscription<T, K> : IDisposable
    {
        public Action<T, K> Action { get; }
        private readonly Action<Subscription<T, K>> OnDispose;

        public Subscription(Action<T, K> action, Action<Subscription<T, K>> onDispose)
        {
            Action = action;
            OnDispose = onDispose;
        }

        public void Dispose()
        {
            OnDispose?.Invoke(this);
        }
    }
}