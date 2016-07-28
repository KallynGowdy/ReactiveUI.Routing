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
using ReactiveUI;
using ReactiveUI.Routing;
using Splat;

namespace ShareNavigation
{
    public class RoutableActivity<T> : ReactiveActivity<T> 
        where T : class
    {
        private readonly IRouter router;

        public RoutableActivity() : this(null) { }
        public RoutableActivity(IRouter router)
        {
            this.router = router ?? Locator.Current.GetService<IRouter>();
        }

        public override void OnBackPressed()
        {
            router.BackAsync();
        }
    }
}