#region

using Runtime.UI.Core;
using UnityEngine;

#endregion

namespace Runtime.UI.Gameplay
{
    public class GameplayPopupService : PopupService
    {
        private readonly GameplayUIRoot _uiRoot;

        protected override Transform PopupLayer => _uiRoot.PopupsLayer;

        public GameplayPopupService(ViewsFactory viewsFactory, ProjectPresentersFactory presentersFactory, GameplayUIRoot uiRoot) : base(viewsFactory,
            presentersFactory)
        {
            _uiRoot = uiRoot;
        }
    }
}