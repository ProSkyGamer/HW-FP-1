#region

using Runtime.UI.Core;

#endregion

namespace Runtime.UI.Common
{
    public class ConfirmPopupPresenter : PopupPresenterBase
    {
        private readonly ConfirmPopupView _view;
        private readonly string _popupText;

        protected override PopupViewBase View => _view;

        public ConfirmPopupPresenter(ConfirmPopupView view, string popupText)
        {
            _view = view;
            _popupText = popupText;
        }

        public override void Initialize()
        {
            base.Initialize();

            _view.SetText(_popupText);
        }
    }
}