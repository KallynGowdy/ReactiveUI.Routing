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
        private readonly Subject<(FragmentManager, Fragment)> fragmentDestroyed = new Subject<(FragmentManager, Fragment)>();
        private readonly Subject<(FragmentManager, Fragment)> fragmentCreated = new Subject<(FragmentManager, Fragment)>();
        private readonly Subject<(FragmentManager, Fragment)> fragmentActivityCreated = new Subject<(FragmentManager, Fragment)>();
        private readonly Subject<(FragmentManager, Fragment)> fragmentStarted = new Subject<(FragmentManager, Fragment)>();
        private readonly Subject<(FragmentManager, Fragment)> fragmentStopped = new Subject<(FragmentManager, Fragment)>();

        public IObservable<(FragmentManager fragmentManager, Fragment fragment)> FragmentResumed => fragmentResumed;
        public IObservable<(FragmentManager fragmentManager, Fragment fragment)> FragmentPaused => fragmentPaused;
        public IObservable<(FragmentManager fragmentManager, Fragment fragment)> FragmentDestroyed => fragmentDestroyed;
        public IObservable<(FragmentManager fragmentManager, Fragment fragment)> FragmentCreated => fragmentCreated;
        public IObservable<(FragmentManager fragmentManager, Fragment fragment)> FragmentActivityCreated => fragmentActivityCreated;
        public IObservable<(FragmentManager fragmentManager, Fragment fragment)> FragmentStarted => fragmentStarted;
        public IObservable<(FragmentManager fragmentManager, Fragment fragment)> FragmentStopped => fragmentStopped;

        public override void OnFragmentActivityCreated(FragmentManager fm, Fragment f, Bundle savedInstanceState)
        {
            base.OnFragmentActivityCreated(fm, f, savedInstanceState);
            fragmentActivityCreated.OnNext((fm, f));
        }

        public override void OnFragmentCreated(FragmentManager fm, Fragment f, Bundle savedInstanceState)
        {
            base.OnFragmentCreated(fm, f, savedInstanceState);
            fragmentCreated.OnNext((fm, f));
        }

        public override void OnFragmentStarted(FragmentManager fm, Fragment f)
        {
            base.OnFragmentStarted(fm, f);
            fragmentStarted.OnNext((fm, f));
        }

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

        public override void OnFragmentStopped(FragmentManager fm, Fragment f)
        {
            base.OnFragmentStopped(fm, f);
            fragmentStopped.OnNext((fm, f));
        }

        public override void OnFragmentDestroyed(FragmentManager fm, Fragment f)
        {
            base.OnFragmentDestroyed(fm, f);
            fragmentDestroyed.OnNext((fm, f));
        }
    }
}