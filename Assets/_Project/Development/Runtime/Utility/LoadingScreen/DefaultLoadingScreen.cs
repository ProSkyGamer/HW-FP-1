#region

using UnityEngine;

#endregion

namespace Runtime.Utility.LoadingScreen
{
    public class DefaultLoadingScreen : MonoBehaviour, ILoadingScreen
    {
        public bool IsShowing => gameObject.activeSelf;

        private void Awake()
        {
            Hide();
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}