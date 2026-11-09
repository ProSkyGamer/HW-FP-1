#region

using System;
using UnityEngine;

#endregion

namespace Runtime.UI.Core
{
    public abstract class PopupPresenterBase : IPresenter
    {
        public event Action<PopupPresenterBase> CloseRequested;
        protected abstract PopupViewBase View { get; }

        public virtual void Initialize()
        {
            Hide();
        }

        protected virtual void OnViewCloseRequested()
        {
            Debug.Log("before event");

            CloseRequested?.Invoke(this);
        }

        public void Show()
        {
            PreShow();

            View.Show();

            PostShow();
        }

        public void Hide()
        {
            PreHide();

            View.Hide();

            PostHide();
        }

        protected virtual void PreShow()
        {
            View.CloseRequested += OnViewCloseRequested;
        }

        protected virtual void PostShow()
        {
        }

        protected virtual void PreHide()
        {
            View.CloseRequested -= OnViewCloseRequested;
        }

        protected virtual void PostHide()
        {
        }

        public virtual void Dispose()
        {
            View.CloseRequested -= OnViewCloseRequested;
        }
    }
}