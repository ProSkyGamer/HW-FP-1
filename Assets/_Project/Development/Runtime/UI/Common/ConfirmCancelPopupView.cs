#region

using System;
using Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Runtime.UI.Common
{
    public class ConfirmCancelPopupView : PopupViewBase
    {
        public event Action PopupConfirmed;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _confirmButton;

        public void SetText(string text)
        {
            _text.text = text;
        }

        private void ConfirmPopup()
        {
            PopupConfirmed?.Invoke();

            RequestHide();
        }

        protected override void PreShow()
        {
            base.PreShow();

            _confirmButton.onClick.AddListener(ConfirmPopup);
        }

        protected override void PreHide()
        {
            base.PreHide();

            _confirmButton.onClick.RemoveListener(ConfirmPopup);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _confirmButton.onClick.RemoveListener(ConfirmPopup);
        }
    }
}