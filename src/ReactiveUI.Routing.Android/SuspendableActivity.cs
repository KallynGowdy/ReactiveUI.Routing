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
        public SuspendableAcitivity(SuspensionNotifierHelper suspensionNotifier)
        {
            this.suspensionNotifier = new Lazy<SuspensionNotifierHelper>(() =>
                suspensionNotifier ?? Locator.Current.GetService<SuspensionNotifierHelper>());
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            SuspensionNotifier?.TriggerSaveState();
        }
    }
}