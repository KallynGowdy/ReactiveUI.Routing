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
using Fragment = Android.Support.V4.App.Fragment;

namespace ReactiveUI.Routing.UseCases.Android
{
    public class DetailPage : Fragment, IViewFor<DetailViewModel>
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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = inflater.Inflate(Resource.Layout.Detail, container, false);

            backToLoginButton = root.FindViewById<Button>(Resource.Id.BackToLoginButton);

            return root;
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (DetailViewModel)value;
        }

        public DetailViewModel ViewModel { get; set; }
    }
}