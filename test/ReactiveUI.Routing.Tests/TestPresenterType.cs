using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Tests
{
    public class TestPresenterType : IPresenter
    {
        public Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            return Task.FromResult<IDisposable>(new BooleanDisposable());
        }
    }
}