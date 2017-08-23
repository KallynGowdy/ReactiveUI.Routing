﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using ReactiveUI.Routing.Android;
using ReactiveUI.Routing.Presentation;
using ReactiveUI.Routing.UseCases.Common.ViewModels;
using Splat;

namespace ReactiveUI.Routing.UseCases.Android
{
    [Activity(Label = "ReactiveUI.Routing.UseCases.Android", MainLauncher = true)]
    public class MainActivity : FragmentActivity, IActivatable
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Locator.CurrentMutable.InitializeRoutingAndroid(this);
            this.WhenActivated(d =>
            {
                PagePresenter.RegisterHost(SupportFragmentManager, Resource.Id.Container)
                    .DisposeWith(d);

                Locator.Current.GetService<IAppPresenter>()
                    .PresentPage(new LoginViewModel())
                    .Subscribe()
                    .DisposeWith(d);
            });

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
        }
    }
}