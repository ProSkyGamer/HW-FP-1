#region

using System;
using Runtime.UI.Core;

#endregion

namespace Runtime.UI.Common
{
    public class ConfirmPopupPresenter : PopupPresenterBase
    {
        private readonly ConfirmPopupView _view;
        private readonly string _popupText;
        private readonly Action _confirmCallback;

        protected override PopupViewBase View => _view;

        public ConfirmPopupPresenter(ConfirmPopupView view, string popupText, Action confirmCallback = null)
        {
            _view = view;
            _popupText = popupText;
            _confirmCallback = confirmCallback;
        }

        protected override void OnViewCloseRequested()
        {
            base.OnViewCloseRequested();

            _confirmCallback?.Invoke();
        }

        public override void Initialize()
        {
            base.Initialize();

            _view.SetText(_popupText);
        }
    }
}