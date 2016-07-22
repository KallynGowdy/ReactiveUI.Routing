using System;
using Android.App;
using Android.Runtime;
using ReactiveUI;

namespace ShareNavigation
{
    [Application]
    public class AndroidApplication : Application
    {
        AutoSuspendHelper autoSuspendHelper;
        public AndroidApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle,transfer)
        {
            autoSuspendHelper = new AutoSuspendHelper(this);
        }
    }
}

