using Android.Support.V4.App;
using Splat;

namespace ReactiveUI.Routing.Android
{
    public static class LocatorExtensions
    {
        public static void InitializeRoutingAndroid(this IMutableDependencyResolver resolver, FragmentActivity mainActivity)
        {
            new Registrations(mainActivity).Register(resolver);
        }
    }
}
