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

namespace ReactiveUI.Routing.Android
{
    public class AndroidToastPresenter : AndroidPresenter, IToastPresenter
    {
        public Task<IDisposable> PresentAsync(IToastViewModel viewModel, object hint)
        {
            ToastPresenterHints hints = hint as ToastPresenterHints ?? new ToastPresenterHints();
            ToastLength duration = ToastLength.Short;
            switch (hints.Duration)
            {
                case ToastPresenterDurationHints.Short:
                    duration = ToastLength.Short;
                    break;
                case ToastPresenterDurationHints.Long:
                    duration = ToastLength.Long;
                    break;
            }
            var toast = Toast.MakeText(Application.Context, viewModel.Message, duration);
            toast.Show();

            return Task.FromResult<IDisposable>(null);
        }

        public override Task<IDisposable> PresentAsync(object viewModel, object hint)
        {
            return PresentAsync((IToastViewModel)viewModel, hint);
        }
    }
}