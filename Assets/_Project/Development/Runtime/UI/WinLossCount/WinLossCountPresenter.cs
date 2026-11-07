#region

using System.Collections.Generic;
using Runtime.Meta.GameStatistics;
using Runtime.UI.Common;
using Runtime.UI.Core;
using Runtime.Utility.Reacts;

#endregion

namespace Runtime.UI.WinLossCount
{
    public class WinLossCountPresenter : IPresenter
    {
        private readonly WinLossCountView _view;
        private readonly TypingMiniGameStatisticsService _statisticsService;
        private readonly ProjectPresentersFactory _projectPresentersFactory;

        private readonly Dictionary<CountTextPresenter, DoubleTextView> _presenterToView = new();

        private readonly ViewsFactory _viewsFactory;

        public WinLossCountPresenter(WinLossCountView view, TypingMiniGameStatisticsService statisticsService,
            ProjectPresentersFactory projectPresentersFactory, ViewsFactory viewsFactory)
        {
            _view = view;
            _statisticsService = statisticsService;
            _projectPresentersFactory = projectPresentersFactory;
            _viewsFactory = viewsFactory;
        }

        public void Initialize()
        {
            CreateViewFor(_statisticsService.WinCount, "Побед");
            CreateViewFor(_statisticsService.LoseCount, "Поражений");
        }

        private void CreateViewFor(IReadonlyVariable<int> variable, string name)
        {
            DoubleTextView variableView = _viewsFactory.CreateView<DoubleTextView>(ViewIDs.DoubleTextElementViewID);
            _view.AddElement(variableView);

            CountTextPresenter variablePresenter = _projectPresentersFactory.CreateCountTextPresenter(variableView, variable, name);
            variablePresenter.Initialize();

            _presenterToView.Add(variablePresenter, variableView);
        }

        private void RemoveViewFor(CountTextPresenter presenter)
        {
            DoubleTextView view = _presenterToView[presenter];
            _view.RemoveElement(view);
            _viewsFactory.DestroyView(view);

            presenter.Dispose();
            _presenterToView.Remove(presenter);
        }

        public void Dispose()
        {
            foreach (CountTextPresenter presenter in _presenterToView.Keys)
            {
                RemoveViewFor(presenter);
            }
        }
    }
}