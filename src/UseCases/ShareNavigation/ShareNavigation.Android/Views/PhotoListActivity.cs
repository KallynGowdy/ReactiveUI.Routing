using System;
using System.Linq;
using System.Reactive.Linq;
using Akavache;
using Android.App;
using Android.OS;
using Android.Widget;
using ReactiveUI;
using ShareNavigation.ViewModels;

namespace ShareNavigation.Views
{
    [Activity(Label = "Starter-Android")]
    public class PhotoListActivity : ReactiveActivity<PhotoListViewModel>
    {

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.PhotoList);
            this.WhenActivated(d =>
            {
                d(ViewModel.WhenAnyValue(vm => vm.LoadedPhotos)
                    .Select(photos => new PhotoListItemAdapter(this, photos))
                    .BindTo(this, view => view.PhotosList.Adapter));
                d(this.BindCommand(ViewModel, vm => vm.Share, view => view.ShareButton, nameof(Button.Click)));
                ViewModel.LoadPhotos.Execute(null);
            });
        }

        private ListView PhotosList => this.GetControl<ListView>();
        private Button ShareButton => this.GetControl<Button>();

    }
}