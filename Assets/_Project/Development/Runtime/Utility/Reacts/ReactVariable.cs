#region

using System;
using System.Collections.Generic;

#endregion

namespace Runtime.Utility.Reacts
{
    public class ReactVariable<T> : IReadonlyVariable<T> where T : IEquatable<T>
    {
        private readonly List<Subscription<T, T>> _toSubscribe;
        private readonly List<Subscription<T, T>> _subscriptions;
        private readonly List<Subscription<T, T>> _toUnsubscribe;

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (value.Equals(_value))
                    return;

                T oldValue = _value;
                _value = value;

                OnValueChanged(oldValue, value);
            }
        }

        public ReactVariable()
        {
            Value = default;

            _toSubscribe = new();
            _subscriptions = new();
            _toUnsubscribe = new();
        }

        public ReactVariable(T value)
        {
            Value = value;

            _toSubscribe = new();
            _subscriptions = new();
            _toUnsubscribe = new();
        }

        private void OnValueChanged(T oldValue, T newValue)
        {
            foreach (Subscription<T, T> toRemoveSubscription in _toUnsubscribe)
            {
                if (_subscriptions.Contains(toRemoveSubscription))
                    _subscriptions.Remove(toRemoveSubscription);
            }

            _toUnsubscribe.Clear();

            foreach (Subscription<T, T> toAddSubscription in _toSubscribe)
            {
                if (_subscriptions.Contains(toAddSubscription) == false)
                    _subscriptions.Add(toAddSubscription);
            }

            _toSubscribe.Clear();

            foreach (Subscription<T, T> subscription in _subscriptions)
            {
                subscription.Action?.Invoke(oldValue, newValue);
            }
        }

        public IDisposable Subscribe(Action<T, T> action)
        {
            Subscription<T, T> subscription = new Subscription<T, T>(action, Remove);
            _toSubscribe.Add(subscription);

            return subscription;
        }

        private void Remove(Subscription<T, T> toRemove)
        {
            _toUnsubscribe.Add(toRemove);
        }
    }
}