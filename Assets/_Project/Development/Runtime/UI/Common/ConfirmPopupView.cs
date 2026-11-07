#region

using Runtime.UI.Core;
using TMPro;
using UnityEngine;

#endregion

namespace Runtime.UI.Common
{
    public class ConfirmPopupView : PopupViewBase
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}