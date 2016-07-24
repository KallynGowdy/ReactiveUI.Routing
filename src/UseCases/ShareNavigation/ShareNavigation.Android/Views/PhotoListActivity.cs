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
    [Activity(Label = "Starter-Android")]
    public class PhotoListActivity : ReactiveActivity<PhotoListActivity>
    {
        int count = 1;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            this.WhenActivated(d =>
            {
                // Bindings
                d(MyButton.Events().Click.Subscribe(_ => MyButton.Text = string.Format("{0} clicks!", count++)));
            });
        }

        public Button MyButton => this.GetControl<Button>();
    }
}