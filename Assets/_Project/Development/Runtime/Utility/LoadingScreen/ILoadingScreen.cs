namespace Runtime.Utility.LoadingScreen
{
    public interface ILoadingScreen
    {
        public bool IsShowing { get; }

        public void Show();
        public void Hide();
    }
}