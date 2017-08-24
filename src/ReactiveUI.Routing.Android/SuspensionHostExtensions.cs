using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReactiveUI.Routing.Android
{
    public static class SuspensionHostExtensions
    {

        public static IDisposable SetupSuspensionPattern(this ISuspensionHost host, Application app)
        {
            var handler = new ActivityLifecycleCallbackHandler();
            app.RegisterActivityLifecycleCallbacks(handler);

            host.IsLaunchingNew = handler.ActivityCreated.Where(args => args.savedInstanceState == null)
                .Select(_ => Unit.Default);
            host.IsResuming = handler.ActivityCreated.Where(args => args.savedInstanceState != null)
                .Select(_ => Unit.Default);
            host.IsUnpausing = handler.ActivityResumed.Select(_ => Unit.Default);
            host.ShouldPersistState = handler.ActivitySaveInstanceState
                .Select(_ => Disposable.Empty);

            return Disposable.Create(() => app.UnregisterActivityLifecycleCallbacks(handler));
        }

    }
}