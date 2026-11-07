#region

using System.Collections.Generic;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.UI.Common;
using Runtime.UI.Core;

#endregion

namespace Runtime.UI.Wallet
{
    public class WalletPresenter : IPresenter
    {
        private readonly IconTextElementListView _view;
        private readonly WalletService _walletService;

        private readonly ViewsFactory _viewFactory;
        private readonly ProjectPresentersFactory _projectPresentersFactory;

        private readonly Dictionary<CurrencyPresenter, IconTextElementView> _presenterToView = new();

        public WalletPresenter(IconTextElementListView view, WalletService walletService, ViewsFactory viewFactory,
            ProjectPresentersFactory projectPresentersFactory)
        {
            _view = view;
            _walletService = walletService;
            _viewFactory = viewFactory;
            _projectPresentersFactory = projectPresentersFactory;
        }

        public void Initialize()
        {
            foreach (CurrencyType currency in _walletService.AvailableCurrencies)
            {
                IconTextElementView currencyView = _viewFactory.CreateView<IconTextElementView>(ViewIDs.IconTextElementViewID);

                CurrencyPresenter currencyPresenter =
                    _projectPresentersFactory.CreateCurrencyPresenter(currencyView, _walletService.GetCurrency(currency), currency);

                currencyPresenter.Initialize();

                _view.AddElement(currencyView);

                _presenterToView.Add(currencyPresenter, currencyView);
            }
        }

        private void DeleteCurrency(CurrencyPresenter presenter)
        {
            IconTextElementView currencyView = _presenterToView[presenter];
            _view.RemoveElement(currencyView);
            _viewFactory.DestroyView(currencyView);

            presenter.Dispose();

            _presenterToView.Remove(presenter);
        }

        public void Dispose()
        {
            foreach (CurrencyPresenter presenter in _presenterToView.Keys)
            {
                DeleteCurrency(presenter);
            }
        }
    }
}