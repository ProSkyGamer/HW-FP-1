#region

using Runtime.UI.Core;
using TMPro;
using UnityEngine;

#endregion

namespace Runtime.UI.MiniGameTypePopup
{
    public class MiniGameTypePopupView : PopupViewBase
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private MiniGameTypeListView _miniGameTypeListView;

        public void SetTitle(string title)
        {
            _title.text = title;
        }

        public void AddElementToView(MiniGameTypeTileView tile)
        {
            _miniGameTypeListView.AddElement(tile);
        }

        public void RemoveElementFromView(MiniGameTypeTileView tile)
        {
            _miniGameTypeListView.RemoveElement(tile);
        }
    }
}