#region

using System;
using Runtime.Gameplay.Meta.Wallet;
using Runtime.UI.Common;
using Runtime.UI.Core;
using Runtime.Utility.Reacts;

#endregion

namespace Runtime.UI.Wallet
{
    public class CurrencyPresenter : IPresenter
    {
        private readonly IconTextElementView _view;
        private readonly IReadonlyVariable<int> _currency;
        private readonly CurrencyType _currencyType;
        private readonly CurrencyIconConfig _currencyIconConfig;

        private IDisposable _subscription;

        public CurrencyPresenter(IconTextElementView view, IReadonlyVariable<int> currency, CurrencyType currencyType,
            CurrencyIconConfig currencyIconConfig)
        {
            _view = view;
            _currency = currency;
            _currencyType = currencyType;
            _currencyIconConfig = currencyIconConfig;
        }

        public void Initialize()
        {
            _view.SetIcon(_currencyIconConfig.GetIconFor(_currencyType));
            _subscription = _currency.Subscribe(OnCurrencyUpdated);

            UpdateCurrency(_currency.Value);
        }

        private void OnCurrencyUpdated(int oldValue, int newValue)
        {
            UpdateCurrency(newValue);
        }

        private void UpdateCurrency(int value)
        {
            _view.SetText(value.ToString());
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}