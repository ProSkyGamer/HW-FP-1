#region

using System;

#endregion

namespace Runtime.Utility.DI
{
    public class Registration
    {
        private readonly Func<DIContainer, object> _creator;
        private object _instance;

        public Registration(Func<DIContainer, object> creator)
        {
            _creator = creator ?? throw new NullReferenceException($"Creator for registration cannot be null!");
        }

        public object CreateInstance(DIContainer container)
        {
            if (_instance != null)
                return _instance;

            if (_creator == null)
                throw new NullReferenceException($"Creator for registration cannot be null!");

            _instance = _creator(container);

            return _instance;
        }
    }
}