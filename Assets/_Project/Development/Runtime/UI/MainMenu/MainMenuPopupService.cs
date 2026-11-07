#region

using Runtime.UI.Core;
using UnityEngine;

#endregion

namespace Runtime.UI.MainMenu
{
    public class MainMenuPopupService : PopupService
    {
        private readonly MainMenuUIRoot _mainMenuUIRoot;

        protected override Transform PopupLayer => _mainMenuUIRoot.PopupsLayer;

        public MainMenuPopupService(ViewsFactory viewsFactory, ProjectPresentersFactory presentersFactory, MainMenuUIRoot mainMenuUIRoot) : base(
            viewsFactory, presentersFactory)
        {
            _mainMenuUIRoot = mainMenuUIRoot;
        }
    }
}