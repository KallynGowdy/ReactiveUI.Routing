using Android.App;
using Android.Content.PM;
using Android.OS;
using ReactiveUI.Routing.Android;
using Splat;

namespace PresentationDemos.Droid
{
    [Activity(Label = "PresentationDemos", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new DefaultAndroidDependencies(this, bundle)));
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            Locator.Current.GetService<AndroidSuspensionNotifierHelper>()?.TriggerSaveState(outState);
        }
    }
}

