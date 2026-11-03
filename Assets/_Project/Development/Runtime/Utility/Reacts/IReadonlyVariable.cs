#region

using System;

#endregion

namespace Runtime.Utility.Reacts
{
    public interface IReadonlyVariable<T>
    {
        public T Value { get; }

        public IDisposable Subscribe(Action<T, T> action);
    }
}