using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Splat;

namespace ReactiveUI.Routing.Android
{
    /// <summary>
    /// Defines a class that implements <see cref="IRoutedAppConfig"/> for Android.
    /// </summary>
    public class DefaultAndroidConfig : IRoutedAppConfig
    {
        private readonly Activity mainActivity;
        private readonly Bundle savedInstanceState;

        public DefaultAndroidConfig(Activity mainActivity, Bundle savedInstanceState)
        {
            if (mainActivity == null) throw new ArgumentNullException(nameof(mainActivity));
            this.mainActivity = mainActivity;
            this.savedInstanceState = savedInstanceState;
        }

        public void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            resolver.RegisterConstant(mainActivity.Application, typeof(Application));
            resolver.RegisterConstant(mainActivity, typeof(Context));
            resolver.Register(() => new AndroidActivityPresenter(), typeof(IPresenter));
            resolver.RegisterLazySingleton(() =>
            {
                var callbacks = new AndroidActivityCallbacks();
                mainActivity.Application.RegisterActivityLifecycleCallbacks(callbacks);
                return callbacks;
            }, typeof(AndroidActivityCallbacks));
            resolver.RegisterLazySingleton(() =>
                Locator.Current.GetService<AndroidActivityCallbacks>().SaveInstanceState
                    .Where(e => e.Activity == mainActivity)
                    .Select(e => e.Bundle)
                    .StartWith(savedInstanceState),
                typeof(IObservable<Bundle>));
            resolver.RegisterLazySingleton(
                () => (SuspensionNotifierHelper)Locator.Current.GetService<ISuspensionNotifier>(),
                typeof(SuspensionNotifierHelper));
            resolver.RegisterLazySingleton(() =>
                new BooleanToViewStateTypeConverter(),
                typeof(IBindingTypeConverter));
        }

        public void CloseApp() => mainActivity.FinishAffinity();
        public IObjectStateStore BuildObjectStateStore() => new AndroidBundleObjectStateStore();
        public ISuspensionNotifier BuildSuspensionNotifier() => new SuspensionNotifierHelper();
    }
}