#region

using System;
using Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Runtime.UI.MiniGameTypePopup
{
    public class MiniGameTypeTileView : MonoBehaviour, IShowableView
    {
        public event Action Clicked;

        [SerializeField] private TextMeshProUGUI _miniGameTypeText;
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetText(string text)
        {
            _miniGameTypeText.text = text;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
}