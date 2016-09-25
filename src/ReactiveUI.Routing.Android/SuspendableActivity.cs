using System;
using Android.App;
using Android.OS;
using Splat;

namespace ReactiveUI.Routing.Android
{
    public class SuspendableAcitivity : Activity
    {
        private readonly Lazy<AndroidSuspensionNotifierHelper> suspensionNotifier;
        protected AndroidSuspensionNotifierHelper SuspensionNotifier => suspensionNotifier.Value;

        public SuspendableAcitivity() : this(null) { }
        public SuspendableAcitivity(AndroidSuspensionNotifierHelper suspensionNotifier)
        {
            this.suspensionNotifier = new Lazy<AndroidSuspensionNotifierHelper>(() =>
                suspensionNotifier ?? Locator.Current.GetService<AndroidSuspensionNotifierHelper>());
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            SuspensionNotifier.SendBundle(outState);
            Locator.Current.GetService<IRoutedAppHost>().SaveStateAsync();
            base.OnSaveInstanceState(outState);
        }
    }
}