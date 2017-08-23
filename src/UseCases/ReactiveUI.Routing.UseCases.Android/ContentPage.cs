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
    [Activity(Label = "ContentPage", NoHistory = true)]
    public class ContentPage : Activity, IViewFor<ContentViewModel>
    {
        private Button showDetailButton;
        private TextView content;

        public ContentPage()
        {
            SetupBindings();
        }

        public ContentPage(IntPtr javaReference, JniHandleOwnership ownership)
            : base(javaReference, ownership)
        {
            SetupBindings();
        }

        private void SetupBindings()
        {
            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Text, view => view.content.Text);
                this.BindCommand(ViewModel, vm => vm.ShowDetail, view => view.showDetailButton);
            });
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Content);

            // Create your application here
            showDetailButton = FindViewById<Button>(Resource.Id.ShowDetailButton);
            content = FindViewById<TextView>(Resource.Id.Content);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ContentViewModel)value;
        }

        public ContentViewModel ViewModel { get; set; }
    }
}