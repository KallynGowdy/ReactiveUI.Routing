using System;
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
using ReactiveUI.Routing.Android.Utils;
using ReactiveUI.Routing.Presentation;
using ReactiveUI.Routing.UseCases.Common;
using ReactiveUI.Routing.UseCases.Common.ViewModels;
using Splat;

namespace ReactiveUI.Routing.UseCases.Android
{
    [Application(
        Debuggable = true,
        Label = "ReactiveUI.Routing.UseCases.Android")]
    public class Application : global::Android.App.Application
    {
        private ApplicationViewModel app;

        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            app = new ApplicationViewModel();
            app.Initialize();
            RegisterViews();

            Locator.CurrentMutable.RegisterConstant(new LogcatLogger("ReactiveUI.Routing.UseCases.Android"), typeof(ILogger));
        }

        public override void OnCreate()
        {
            RxApp.SuspensionHost.SetupSuspensionPattern(this);
            base.OnCreate();


            //RxApp.SuspensionHost.WhenAnyValue(h => h.AppState)
            //    .Cast<ReactiveAppState>()
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Do(state => app.LoadState(state))
            //    .Subscribe();
            //RxApp.SuspensionHost.SetupPersistence(() => app.BuildAppState(), new Store<ReactiveAppState>(this));
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginViewModel>));
            Locator.CurrentMutable.Register(() => new DetailPage(), typeof(IViewFor<DetailViewModel>));
            Locator.CurrentMutable.Register(() => new ContentPage(), typeof(IViewFor<ContentViewModel>));
        }
    }
}