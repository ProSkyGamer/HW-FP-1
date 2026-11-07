#region

using System;
using Runtime.UI.Common;
using Runtime.UI.Core;
using Runtime.UI.WinLossCount;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action ChooseMiniGameTypeButtonPressed;
        public event Action ResetGameStatsButtonPressed;

        [SerializeField] private Button _chooseMiniGameTypeButton;
        [SerializeField] private Button _resetGameStatsButton;
        [field: SerializeField] public IconTextElementListView WalletView { get; private set; }
        [field: SerializeField] public WinLossCountView WinLossCountView { get; private set; }

        private void OnEnable()
        {
            _chooseMiniGameTypeButton.onClick.AddListener(ChooseMiniGameTypeButtonClick);
            _resetGameStatsButton.onClick.AddListener(ResetGameStatsButtonClick);
        }

        private void ResetGameStatsButtonClick()
        {
            ResetGameStatsButtonPressed?.Invoke();
        }

        private void ChooseMiniGameTypeButtonClick()
        {
            ChooseMiniGameTypeButtonPressed?.Invoke();
        }

        private void OnDisable()
        {
            _chooseMiniGameTypeButton.onClick.RemoveListener(ChooseMiniGameTypeButtonClick);
            _resetGameStatsButton.onClick.RemoveListener(ResetGameStatsButtonClick);
        }
    }
}