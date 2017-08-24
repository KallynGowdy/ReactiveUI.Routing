using Android.Support.V4.App;
using Splat;

namespace ReactiveUI.Routing.Android
{
    public static class LocatorExtensions
    {
        private static bool initialized = false;

        private static bool Initialized => !Splat.ModeDetector.InUnitTestRunner() && initialized;

        public static void InitializeRoutingAndroid(this IMutableDependencyResolver resolver, FragmentActivity mainActivity)
        {
            if (!Initialized)
            {
                new Registrations(mainActivity).Register(resolver);
                initialized = true;
            }
        }
    }
}
