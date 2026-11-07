#region

using Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Runtime.UI.Common
{
    public class IconTextElementView : MonoBehaviour, IView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }
    }
}