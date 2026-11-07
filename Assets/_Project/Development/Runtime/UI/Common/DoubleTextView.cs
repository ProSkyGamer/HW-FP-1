#region

using Runtime.UI.Core;
using TMPro;
using UnityEngine;

#endregion

namespace Runtime.UI.Common
{
    public class DoubleTextView : MonoBehaviour, IView
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _valueText;

        public void SetName(string name)
        {
            _nameText.text = name;
        }

        public void SetValue(string value)
        {
            _valueText.text = value;
        }
    }
}