using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Android.App;
using Android.OS;
using Android.Widget;
using ReactiveUI;
using ReactiveUI.Routing.Android;
using ShareNavigation.ViewModels;

namespace ShareNavigation.Views
{
    [Activity(Label = "Starter-Android")]
    public class PhotoListActivity : RoutableActivity<PhotoListViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.PhotoList);
            this.WhenActivated(d =>
            {
                PhotosList.Adapter = new PhotoListItemAdapter(this, ViewModel);
                d(this.BindCommand(ViewModel, vm => vm.Share, view => view.ShareButton, nameof(Button.Click)));
                ViewModel.LoadPhotos.Execute(null);
            });
        }

        private ListView PhotosList => this.GetControl<ListView>();
        private Button ShareButton => this.GetControl<Button>();

    }
}