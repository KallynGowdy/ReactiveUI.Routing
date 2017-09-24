using System;
using Android.App;
using Android.Util;
using ReactiveUI.Routing.Configuration;
using Splat;

namespace ReactiveUI.Routing.Android
{
    public class AndroidConfiguration : IReactiveAppConfiguration
    {
        private readonly Application androidApp;
        public SuspensionConfiguration Suspension { get; } = new SuspensionConfiguration();

        public AndroidConfiguration(Application application)
        {
            androidApp = application ?? throw new ArgumentNullException(nameof(application));
        }

        public void Configure(IReactiveApp application)
        {
            if (application?.Locator != null)
            {
                var activityCallbackHandler = application.Locator.GetService<ActivityLifecycleCallbackHandler>();
                if (activityCallbackHandler != null)
                {
                    androidApp.RegisterActivityLifecycleCallbacks(activityCallbackHandler);
                }

                if (Suspension != null)
                {
                    if (Suspension.SetupPersistance)
                    {
                        application.SuspensionHost.SetupAndroidSuspensionPattern(androidApp, activityCallbackHandler);
                    }

                    Suspension.Configure(application);
                }
            }
        }
    }
}