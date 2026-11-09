#region

using System;
using Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Runtime.UI.Gameplay
{
    public class GameplayScreenView : MonoBehaviour, IView
    {
        public event Action BackToMenuClicked;

        [SerializeField] private TextMeshProUGUI _requiredText;
        [SerializeField] private TextMeshProUGUI _currentText;
        [SerializeField] private Button _backToMenuButton;

        public void OnEnable()
        {
            _backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
        }

        private void OnBackToMenuClicked()
        {
            BackToMenuClicked?.Invoke();
        }

        public void SetRequiredText(string requiredText)
        {
            _requiredText.text = requiredText;
        }

        public void SetCurrentText(string currentText)
        {
            _currentText.text = currentText;
        }

        private void OnDisable()
        {
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuClicked);
        }

        private void OnDestroy()
        {
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuClicked);
        }
    }
}