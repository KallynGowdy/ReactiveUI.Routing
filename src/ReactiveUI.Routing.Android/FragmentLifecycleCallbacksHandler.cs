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
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace ReactiveUI.Routing.Android
{
    public class FragmentLifecycleCallbacksHandler : FragmentManager.FragmentLifecycleCallbacks
    {
        private readonly Subject<(FragmentManager, Fragment)> fragmentResumed = new Subject<(FragmentManager, Fragment)>();
        private readonly Subject<(FragmentManager, Fragment)> fragmentPaused = new Subject<(FragmentManager, Fragment)>();

        public IObservable<(FragmentManager fragmentManager, Fragment fragment)> FragmentResumed => fragmentResumed;
        public IObservable<(FragmentManager fragmentManager, Fragment fragment)> FragmentPaused => fragmentPaused;

        public override void OnFragmentResumed(FragmentManager fm, Fragment f)
        {
            base.OnFragmentStarted(fm, f);
            fragmentResumed.OnNext((fm, f));
        }

        public override void OnFragmentPaused(FragmentManager fm, Fragment f)
        {
            base.OnFragmentPaused(fm, f);
            fragmentPaused.OnNext((fm, f));
        }
    }
}