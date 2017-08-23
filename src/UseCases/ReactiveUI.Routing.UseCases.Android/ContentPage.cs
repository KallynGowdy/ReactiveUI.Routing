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
    public class ContentPage : Fragment, IViewFor<ContentViewModel>
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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = inflater.Inflate(Resource.Layout.Content, container, false);

            showDetailButton = root.FindViewById<Button>(Resource.Id.ShowDetailButton);
            content = root.FindViewById<TextView>(Resource.Id.Content);

            return root;
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ContentViewModel)value;
        }

        public ContentViewModel ViewModel { get; set; }
    }
}