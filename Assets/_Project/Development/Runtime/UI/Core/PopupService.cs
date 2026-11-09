#region

using System;
using System.Collections.Generic;
using Runtime.UI.Common;
using Runtime.UI.MiniGameTypePopup;
using UnityEngine;

#endregion

namespace Runtime.UI.Core
{
    public abstract class PopupService : IDisposable
    {
        private readonly ViewsFactory _viewsFactory;
        private readonly ProjectPresentersFactory _presentersFactory;

        private readonly Dictionary<PopupPresenterBase, PopupViewBase> _presenterToView = new();

        protected PopupService(ViewsFactory viewsFactory, ProjectPresentersFactory presentersFactory)
        {
            _viewsFactory = viewsFactory;
            _presentersFactory = presentersFactory;
        }

        protected abstract Transform PopupLayer { get; }

        public void ShowMiniGameTypePopup()
        {
            MiniGameTypePopupView view = _viewsFactory.CreateView<MiniGameTypePopupView>(ViewIDs.MiniGamePopupViewID, PopupLayer);

            MiniGameTypePopupPresenter presenter = _presentersFactory.CreateMiniGameTypePopupPresenter(view);

            InitializePresenter(presenter, view);
        }

        public void ShowConfirmCancelPopup(Action confirmCallback, string popupText)
        {
            ConfirmCancelPopupView view = _viewsFactory.CreateView<ConfirmCancelPopupView>(ViewIDs.ConfirmCancelPopupViewID, PopupLayer);
            ConfirmCancelPopupPresenter presenter = _presentersFactory.CreateConfirmCancelPopupPresenter(view, confirmCallback, popupText);

            InitializePresenter(presenter, view);
        }

        public void ShowConfirmPopup(string popupText, Action confirmCallback = null)
        {
            ConfirmPopupView view = _viewsFactory.CreateView<ConfirmPopupView>(ViewIDs.ConfirmPopupViewID, PopupLayer);
            ConfirmPopupPresenter presenter = _presentersFactory.CreateConfirmPopupPresenter(view, popupText, confirmCallback);

            InitializePresenter(presenter, view);
        }

        private void InitializePresenter(PopupPresenterBase presenter, PopupViewBase view)
        {
            presenter.CloseRequested += OnCloseRequested;

            _presenterToView.Add(presenter, view);

            presenter.Initialize();
            presenter.Show();
        }

        private void OnCloseRequested(PopupPresenterBase presenter)
        {
            Debug.Log($"OnCloseRequested {presenter}");

            presenter.CloseRequested -= OnCloseRequested;

            CloseView(presenter);
        }

        public void CloseView(PopupPresenterBase presenter)
        {
            if (_presenterToView.TryGetValue(presenter, out PopupViewBase view) == false)
                return;

            _viewsFactory.DestroyView(view);
            presenter.Dispose();

            _presenterToView.Remove(presenter);
        }

        public void Dispose()
        {
            foreach (PopupPresenterBase presenter in _presenterToView.Keys)
            {
                CloseView(presenter);
            }
        }
    }
}