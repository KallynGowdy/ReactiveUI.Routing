using System;
using System.Reactive.Linq;
using Akavache;
using Android.App;
using Android.OS;
using Android.Widget;
using ReactiveUI;
using ShareNavigation.ViewModels;

namespace ShareNavigation.Views
{
    [Activity (Label = "Starter-Android", MainLauncher = true)]
    public class TestActivity : ReactiveActivity<TestViewModel>
    {
        int count = 1;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            MyButton.Events().Click.Subscribe(_ => MyButton.Text = string.Format("{0} clicks!", count++));

            this.OneWayBind(ViewModel, x => x.TheGuid, x => x.TheGuid.Text);

            ViewModel = await BlobCache.LocalMachine.GetOrCreateObject("TestViewModel", () => new TestViewModel());
        }

        public TextView TheGuid => this.GetControl<TextView>();

        public Button MyButton => this.GetControl<Button>();
    }
}