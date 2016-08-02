using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Defines a class that registers Android-specific dependencies.
    /// </summary>
    public class AndroidRegistrations : IRegisterDependencies
    {
        private readonly Activity mainActivity;

        private AndroidRegistrations(Activity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        void IRegisterDependencies.RegisterDependencies(IMutableDependencyResolver resolver)
        {
            resolver.RegisterConstant(mainActivity.Application, typeof(Application));
            resolver.RegisterConstant(mainActivity, typeof(Context));
            resolver.Register(() => new AndroidActivityPresenter(), typeof(IPresenter));
            resolver.RegisterLazySingleton(() => new SuspensionNotifierHelper(), typeof(SuspensionNotifierHelper));
            resolver.RegisterLazySingleton(() => new BooleanToViewStateTypeConverter(), typeof(IBindingTypeConverter));
        }

        /// <summary>
        /// Registers Android-specific dependencies for an app that runs from the given activity.
        /// </summary>
        /// <param name="mainActivity"></param>
        /// <param name="resolver"></param>
        public static void RegisterDependencies(Activity mainActivity, IMutableDependencyResolver resolver)
        {
            if (mainActivity == null) throw new ArgumentNullException(nameof(mainActivity));
            IRegisterDependencies registrations = new AndroidRegistrations(mainActivity);
            registrations.RegisterDependencies(resolver);
        }
    }
}