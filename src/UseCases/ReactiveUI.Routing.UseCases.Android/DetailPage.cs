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
    [Activity(Label = "DetailPage", NoHistory = true)]
    public class DetailPage : Activity, IViewFor<DetailViewModel>
    {
        private Button backToLoginButton;

        public DetailPage()
        {
            SetupBindings();
        }

        public DetailPage(IntPtr javaReference, JniHandleOwnership ownership) : base(javaReference, ownership)
        {
            SetupBindings();
        }

        private void SetupBindings()
        {
            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.BackToLogin, view => view.backToLoginButton);
            });
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Detail);
            
            backToLoginButton = FindViewById<Button>(Resource.Id.BackToLoginButton);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (DetailViewModel) value;
        }

        public DetailViewModel ViewModel { get; set; }
    }
}