#region

using System;
using Runtime.UI.Core;
using UnityEngine;

#endregion

namespace Runtime.UI.Common
{
    public class ConfirmCancelPopupPresenter : PopupPresenterBase
    {
        private readonly ConfirmCancelPopupView _view;
        private readonly Action _confirmCallback;
        private readonly string _popupText;

        protected override PopupViewBase View => _view;

        public ConfirmCancelPopupPresenter(ConfirmCancelPopupView view, Action confirmCallback, string popupText)
        {
            _view = view;
            _confirmCallback = confirmCallback;
            _popupText = popupText;
        }

        public override void Initialize()
        {
            Debug.Log("initialized");

            base.Initialize();

            _view.SetText(_popupText);
        }

        protected override void PreShow()
        {
            base.PreShow();

            _view.PopupConfirmed += OnViewPopupConfirmed;
        }

        protected override void PreHide()
        {
            base.PreHide();

            _view.PopupConfirmed -= OnViewPopupConfirmed;
        }

        private void OnViewPopupConfirmed()
        {
            _confirmCallback?.Invoke();
        }

        public override void Dispose()
        {
            base.Dispose();
            _view.PopupConfirmed -= OnViewPopupConfirmed;
        }
    }
}