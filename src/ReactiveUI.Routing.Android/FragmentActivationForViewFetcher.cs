﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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
    public class FragmentActivationForViewFetcher : IActivationForViewFetcher
    {
        private readonly FragmentLifecycleCallbacksHandler handler = new FragmentLifecycleCallbacksHandler();
        private FragmentManager manager;

        public FragmentActivationForViewFetcher(FragmentManager manager)
        {
            SetFragmentManager(manager);
        }

        public void SetFragmentManager(FragmentManager manager)
        {
            if (manager != this.manager)
            {
                this.manager?.UnregisterFragmentLifecycleCallbacks(handler);
                this.manager = manager;
                this.manager.RegisterFragmentLifecycleCallbacks(handler, true);
            }
        }

        public int GetAffinityForView(Type view)
        {
            if (typeof(Fragment).IsAssignableFrom(view))
            {
                return 100;
            }
            return 0;
        }

        public IObservable<bool> GetActivationForView(IActivatable view)
        {
            var activity = (Fragment)view;
            return Observable.Merge(
                handler.FragmentActivityCreated.Where(a => a.fragment == activity).Select(a => true),
                handler.FragmentDestroyed.Where(a => a.fragment == activity).Select(a => false));
        }
    }
}