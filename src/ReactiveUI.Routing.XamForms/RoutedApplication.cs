﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ReactiveUI.Routing.XamForms
{
    /// <summary>
    /// Defines a class that represents a <see cref="Application"/> that builds a <see cref="IRoutedAppConfig"/> and starts the <see cref="RoutedAppHost"/>.
    /// </summary>
    public class RoutedApplication : Application, ISuspensionNotifier
    {
        private readonly Subject<Unit> onSaveState = new Subject<Unit>();
        private readonly Subject<Unit> onSuspend = new Subject<Unit>();
        private readonly RoutedAppHost host;

        public RoutedApplication(IRoutedAppConfig appConfig)
        {
            if (appConfig == null) throw new ArgumentNullException(nameof(appConfig));
            host = new RoutedAppHost(appConfig);
        }

        protected override void OnStart()
        {
            base.OnStart();
            host.Start();
        }

        protected override void OnResume()
        {
            base.OnResume();
            host.Start();
        }

        protected override void OnSleep()
        {
            onSaveState.OnNext(Unit.Default);
            base.OnSleep();
            onSuspend.OnNext(Unit.Default);
        }

        public IObservable<Unit> OnSaveState => onSaveState;
        public IObservable<Unit> OnSuspend => onSuspend;
    }
}
