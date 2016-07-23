using System;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Tests
{
    public class OtherTestPresenterType : IPresenter
    {
        public Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            throw new NotImplementedException();
        }
    }
}