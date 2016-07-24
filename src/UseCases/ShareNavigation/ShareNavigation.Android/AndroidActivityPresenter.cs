using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI.Routing;

namespace ShareNavigation
{
    public class AndroidActivityPresenter<TViewModel> : BasePresenter<TViewModel>
    {
        public override Task<IDisposable> PresentAsync(TViewModel viewModel, object hint)
        {
            throw new NotImplementedException();
        }
    }
}