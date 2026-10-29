#region

using System;
using System.Collections.Generic;

#endregion

namespace Runtime.Utility.DI
{
    public class DIContainer
    {
        private readonly Dictionary<Type, Registration> _registrations = new();

        private readonly DIContainer _parent;

        private readonly List<Type> _requests = new();

        public DIContainer()
        {
            _parent = null;
        }

        public DIContainer(DIContainer parent)
        {
            _parent = parent;
        }

        public void AddAsSingle<T>(Func<DIContainer, T> creator)
        {
            if (IsAlreadyRegistered<T>())
                throw new InvalidOperationException($"Type {typeof(T)} is already registered!");

            Registration registration = new(container => creator.Invoke(container));
            _registrations.Add(typeof(T), registration);
        }

        public bool IsAlreadyRegistered<T>()
        {
            if (_registrations.ContainsKey(typeof(T)))
                return true;

            if (_parent != null)
                return _parent.IsAlreadyRegistered<T>();

            return false;
        }

        public T Resolve<T>()
        {
            if (_requests.Contains(typeof(T)))
                throw new InvalidOperationException($"Found cyclical references for {typeof(T).Name}");

            _requests.Add(typeof(T));

            try
            {
                if (_registrations.TryGetValue(typeof(T), out Registration registration))
                    return (T)registration.CreateInstance(this);

                if (_parent != null)
                    return _parent.Resolve<T>();
            }
            finally
            {
                _requests.Remove(typeof(T));
            }

            throw new InvalidOperationException($"No implementation of type {typeof(T)}");
        }
    }
}