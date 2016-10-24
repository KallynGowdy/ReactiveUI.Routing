using System;
using System.Linq;
using System.Reactive;
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
    [Activity(Label = "Photos!")]
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
                d(this.OneWayBind(ViewModel, vm => vm.IsLoading, view => view.ProgressBar.Visibility));
                d(Observable.FromEventPattern<AdapterView.ItemClickEventArgs>(h => this.PhotosList.ItemClick += h,
                    h => this.PhotosList.ItemClick -= h)
                    .Select(args => args.EventArgs.Position)
                    .InvokeCommand(ViewModel, vm => vm.ShowPhoto));
                ViewModel.LoadPhotos.Execute(Unit.Default);
            });
        }

        private ListView PhotosList => this.GetControl<ListView>();
        private Button ShareButton => this.GetControl<Button>();
        private ProgressBar ProgressBar => this.GetControl<ProgressBar>();

    }
}