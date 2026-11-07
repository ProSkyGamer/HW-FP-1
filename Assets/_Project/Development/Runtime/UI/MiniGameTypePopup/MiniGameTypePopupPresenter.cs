#region

using System;
using System.Collections.Generic;
using Runtime.Gameplay.Bootstrap;
using Runtime.UI.Core;

#endregion

namespace Runtime.UI.MiniGameTypePopup
{
    public class MiniGameTypePopupPresenter : PopupPresenterBase
    {
        private const string WindowTitle = "ВЫБОР РЕЖИМА ИГРЫ";

        private readonly MiniGameTypePopupView _view;
        private readonly ViewsFactory _viewFactory;
        private readonly ProjectPresentersFactory _projectPresentersFactory;

        private readonly Dictionary<MiniGameTypeTilePresenter, MiniGameTypeTileView> _presenterToView = new();

        protected override PopupViewBase View => _view;

        public MiniGameTypePopupPresenter(MiniGameTypePopupView view, ViewsFactory viewFactory, ProjectPresentersFactory projectPresentersFactory)
        {
            _view = view;
            _viewFactory = viewFactory;
            _projectPresentersFactory = projectPresentersFactory;
        }

        public override void Initialize()
        {
            base.Initialize();

            _view.SetTitle(WindowTitle);

            foreach (MiniGameType miniGameType in Enum.GetValues(typeof(MiniGameType)))
            {
                MiniGameTypeTileView tileView = _viewFactory.CreateView<MiniGameTypeTileView>(ViewIDs.MiniGameTileViewID);
                MiniGameTypeTilePresenter tilePresenter = _projectPresentersFactory.CreateMiniGameTypeTilePresenter(tileView, miniGameType);
                tilePresenter.Initialize();

                _view.AddElementToView(tileView);

                _presenterToView.Add(tilePresenter, tileView);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (MiniGameTypeTilePresenter presenter in _presenterToView.Keys)
            {
                MiniGameTypeTileView tileView = _presenterToView[presenter];
                _view.RemoveElementFromView(tileView);
                _viewFactory.DestroyView(tileView);

                presenter.Dispose();
            }

            _presenterToView.Clear();
        }
    }
}