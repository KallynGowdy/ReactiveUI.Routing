using System;
using Android.App;
using Android.OS;
using Splat;

namespace ReactiveUI.Routing.Android
{
    public class SuspendableAcitivity : Activity
    {
        private readonly Lazy<SuspensionNotifierHelper> suspensionNotifier;
        protected SuspensionNotifierHelper SuspensionNotifier => suspensionNotifier.Value;

        public SuspendableAcitivity() : this(null) { }
        public SuspendableAcitivity(SuspensionNotifierHelper supensionNotifier)
        {
            this.suspensionNotifier = new Lazy<SuspensionNotifierHelper>(() =>
                supensionNotifier ?? Locator.Current.GetService<SuspensionNotifierHelper>());
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            SuspensionNotifier.TriggerSaveState();
        }
    }
}