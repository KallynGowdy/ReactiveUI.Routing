using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AndroidSuspensionNotifierHelper : SuspensionNotifierHelper
    {
        Subject<Bundle> bundleSubject = new Subject<Bundle>();

        public IObservable<Bundle> LatestBundle => bundleSubject;

        public AndroidSuspensionNotifierHelper(Bundle savedInstanceState = null)
        {
            SendBundle(savedInstanceState);
        }

        public void SendBundle(Bundle bundle)
        {
            if (bundle == null) return;
            bundleSubject.OnNext(bundle);
        }

        public void TriggerSaveState(Bundle bundle)
        {
            SendBundle(bundle);
            base.TriggerSaveState();
        }
    }
}