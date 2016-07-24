using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    public abstract class BasePresenter<TViewModel> : IPresenter<TViewModel>
    {
        public abstract Task<IDisposable> PresentAsync(TViewModel viewModel, object hint);

        public Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            return PresentAsync((TViewModel)viewModel, hint);
        }
    }
}
