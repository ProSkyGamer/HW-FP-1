#region

using System;
using Runtime.UI.Core;
using Runtime.Utility.Reacts;

#endregion

namespace Runtime.UI.Common
{
    public class CountTextPresenter : IPresenter
    {
        private readonly IReadonlyVariable<int> _countVariable;
        private readonly string _countText;
        private readonly DoubleTextView _view;

        private IDisposable _subscription;

        public CountTextPresenter(IReadonlyVariable<int> countVariable, string countText, DoubleTextView view)
        {
            _countVariable = countVariable;
            _countText = countText;
            _view = view;
        }

        public void Initialize()
        {
            _view.SetName(_countText);

            _subscription = _countVariable.Subscribe(OnValueChanged);

            UpdateValue(_countVariable.Value);
        }

        private void OnValueChanged(int oldValue, int newValue)
        {
            UpdateValue(newValue);
        }

        private void UpdateValue(int value)
        {
            _view.SetValue(value.ToString());
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}