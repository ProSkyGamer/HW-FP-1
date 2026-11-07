#region

using System;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Runtime.UI.Core
{
    public class PopupViewBase : MonoBehaviour, IShowableView
    {
        public event Action CloseRequested;

        [SerializeField] private Transform _mainView;
        [SerializeField] private Button _antiClickerButton;
        [SerializeField] private Button _closeButton;

        public void RequestHide()
        {
            CloseRequested?.Invoke();
        }

        public void Show()
        {
            PreShow();

            gameObject.SetActive(true);

            PostShow();
        }

        public void Hide()
        {
            PreHide();

            gameObject.SetActive(false);

            PostHide();
        }

        protected virtual void PreShow()
        {
            _antiClickerButton.onClick.AddListener(RequestHide);
            _closeButton.onClick.AddListener(RequestHide);
        }

        protected virtual void PostShow()
        {
        }

        protected virtual void PreHide()
        {
            _antiClickerButton.onClick.RemoveListener(RequestHide);
            _closeButton.onClick.RemoveListener(RequestHide);
        }

        protected virtual void PostHide()
        {
        }

        protected virtual void OnDestroy()
        {
            _antiClickerButton.onClick.RemoveListener(RequestHide);
            _closeButton.onClick.RemoveListener(RequestHide);
        }
    }
}