using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI.Routing.UseCases.Common.ViewModels;
using Fragment = Android.Support.V4.App.Fragment;

namespace ReactiveUI.Routing.UseCases.Android
{
    public class LoginPage : Fragment, IViewFor<LoginViewModel>
    {
        private Button loginButton;

        private void SetupBindings()
        {
            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.Login, view => view.loginButton)
                    .DisposeWith(d);
            });
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            SetupBindings();
            var view = inflater.Inflate(Resource.Layout.Login, container, false);
            loginButton = view.FindViewById<Button>(Resource.Id.LoginButton);
            return view;
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (LoginViewModel)value;
        }

        public LoginViewModel ViewModel { get; set; }
    }
}