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
using ReactiveUI.Routing.UseCases.Common.ViewModels;

namespace ReactiveUI.Routing.UseCases.Android
{
    [Activity(Label = "ReactiveUI.Routing.UseCases.Android", MainLauncher = true, NoHistory = true)]
    public class LoginPage : Activity, IViewFor<LoginViewModel>
    {
        private Button loginButton;

        public LoginPage()
        {
            SetupBindings();
        }

        public LoginPage(IntPtr javaReference, JniHandleOwnership ownership)
            : base(javaReference, ownership)
        {
            SetupBindings();
        }

        private void SetupBindings()
        {
            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.Login, view => view.loginButton);
            });
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            loginButton = FindViewById<Button>(Resource.Id.LoginButton);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (LoginViewModel)value;
        }

        public LoginViewModel ViewModel { get; set; } = new LoginViewModel();
    }
}