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
        /// <summary>
        /// Configures the <see cref="ISuspensionHost"/> with a suspension pattern that works on android devices.
        /// Different from using <see cref="AutoSuspendHelper"/> because <see cref="AutoSuspendHelper"/> triggers state 
        /// persistance on both ActivitySaveInstanceState and OnPause whereas this only triggers state persistance on ActivitySaveInstanceState.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IDisposable SetupAndroidSuspensionPattern(this ISuspensionHost host, Application app, ActivityLifecycleCallbackHandler handler = null)
        {
            if (handler == null)
            {
                handler = new ActivityLifecycleCallbackHandler();
                app.RegisterActivityLifecycleCallbacks(handler);
            }

            host.IsLaunchingNew = handler.ActivityCreated
                .Where(args => args.savedInstanceState == null)
                .Select(_ => Unit.Default);
            host.IsResuming = handler.ActivityCreated
                .Where(args => args.savedInstanceState != null)
                .Do(args => AutoSuspendHelper.LatestBundle = args.savedInstanceState)
                .Select(_ => Unit.Default);
            host.IsUnpausing = handler.ActivityResumed
                .Select(_ => Unit.Default);
            host.ShouldPersistState = handler.ActivitySaveInstanceState
                .Do(args => AutoSuspendHelper.LatestBundle = args.outState)
                .Select(_ => Disposable.Empty);

            return Disposable.Create(() => app.UnregisterActivityLifecycleCallbacks(handler));
        }

    }
}