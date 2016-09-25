using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReactiveUI.Routing.Android
{
    public class AndroidSuspensionNotifierHelper
    {
        private Bundle latestBundle;

        public Bundle LatestBundle
        {
            get { return latestBundle; }
            private set { latestBundle = value; }
        }

        public AndroidSuspensionNotifierHelper(Bundle savedInstanceState = null)
        {
            SendBundle(savedInstanceState);
        }

        public void SendBundle(Bundle bundle)
        {
            if (bundle != null)
            {
                LatestBundle = bundle;
            }
        }
    }
}