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
using ReactiveUI.Routing.Android;
using ReactiveUI.Routing.Presentation;
using ReactiveUI.Routing.UseCases.Common;
using ReactiveUI.Routing.UseCases.Common.ViewModels;
using Splat;

namespace ReactiveUI.Routing.UseCases.Android
{
    [Application(
        Debuggable = true,
        Label = "ReactiveUI.Routing.UseCases.Android")]
    public class Application : global::Android.App.Application, global::Android.App.Application.IActivityLifecycleCallbacks
    {
        private AutoSuspendHelper suspendHelper;
        private ApplicationViewModel app;

        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            app = new ApplicationViewModel();
            app.Initialize();
            RegisterViews();
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            this.RegisterActivityLifecycleCallbacks(this);
            suspendHelper = new AutoSuspendHelper(this);
            RxApp.SuspensionHost.WhenAnyValue(h => h.AppState)
                .Cast<AppState>()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(state => app.LoadState(state))
                .Subscribe();
            RxApp.SuspensionHost.SetupPersistence(() => app.BuildAppState(), new Store<AppState>(this));
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginViewModel>));
            Locator.CurrentMutable.Register(() => new DetailPage(), typeof(IViewFor<DetailViewModel>));
            Locator.CurrentMutable.Register(() => new ContentPage(), typeof(IViewFor<ContentViewModel>));
        }
    }
}