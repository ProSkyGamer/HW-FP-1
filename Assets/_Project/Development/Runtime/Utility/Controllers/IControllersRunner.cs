#region

using System;

#endregion

namespace Runtime.Utility.Controllers
{
    public interface IControllersRunner
    {
        public void Register(Controller controller, Func<bool> destructionReason);

        public void Remove(Controller controller);
    }
}