using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using ShareNavigation.ViewModels;

namespace ShareNavigation.Views
{
    [Activity(Label = "Share!")]
    public class ShareActivity : ReactiveActivity<ShareViewModel>
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Share);
            this.WhenActivated(d =>
            {
                d(this.Bind(ViewModel, vm => vm.PhotoUrl, view => view.PhotoUrl.Text));
                d(this.BindCommand(ViewModel, vm => vm.Share, view => view.ShareButton));
            });
        }

        private EditText PhotoUrl => this.GetControl<EditText>();
        private Button ShareButton => this.GetControl<Button>();

    }
}