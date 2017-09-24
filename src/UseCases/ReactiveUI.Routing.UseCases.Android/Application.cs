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
        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            var app = new ReactiveAppBuilder()
                .AddReactiveRouting()
                .Add(new CommonUseCaseDependencies())
                .Add(new AndroidViewDependencies())
                .ConfigureAndroid(this)
                .Build();
            base.OnCreate();

        }
    }
}