namespace Runtime.Utility.Controllers
{
    public abstract class Controller
    {
        public bool IsActive { get; private set; }

        public bool IsNeedToBeDestroyed { get; private set; }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Update(float deltaTime)
        {
            if (IsActive == false)
                return;

            ProcessUpdateInternal(deltaTime);
        }

        public virtual void Destroy()
        {
            IsNeedToBeDestroyed = true;
        }

        protected abstract void ProcessUpdateInternal(float deltaTime);
    }
}