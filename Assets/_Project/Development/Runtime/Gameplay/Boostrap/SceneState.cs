namespace Runtime.Gameplay.Bootstrap
{
    public class SceneState
    {
        public bool IsPlaying { get; private set; }

        public void Start()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }
    }
}