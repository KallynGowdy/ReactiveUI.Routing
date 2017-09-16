using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using Splat;

namespace ReactiveUI.Routing.Configuration
{
    /// <summary>
    /// Defines a class that is able to setup a cross-platform compatible suspend/resume pattern
    /// for the application.
    /// </summary>
    public class SuspensionConfiguration : IReactiveAppConfiguration
    {
        /// <summary>
        /// Whether persistance should be setup.
        /// </summary>
        public bool SetupPersistance { get; set; } = true;

        public void Configure(IReactiveApp application)
        {
            if (SetupPersistance)
            {
                application.RegisterDisposable(Setup(application));
            }
        }

        private IDisposable Setup(IReactiveApp application)
        {
            var ret = new CompositeDisposable();

            SetupApp(application, ret);
            SetupSuspensionDriver(application, ret);

            return ret;
        }

        private void SetupApp(IReactiveApp application, CompositeDisposable ret)
        {
            ret.Add(application.SuspensionHost.WhenAnyValue(h => h.AppState)
                .Where(state => state != null)
                .Cast<ReactiveAppState>()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(application.LoadState)
                .Subscribe());
        }

        private static void SetupSuspensionDriver(IReactiveApp application, CompositeDisposable ret)
        {
            var driver = application.SuspensionDriver;
            var host = application.SuspensionHost;

            host.CreateNewAppState = application.BuildAppState;

            ret.Add(host.ShouldInvalidateState
                .SelectMany(_ => driver.InvalidateState())
                .LoggedCatch(host, Observable.Return(Unit.Default), "Tried to invalidate app state")
                .Subscribe(_ => host.Log().Info("Invalidated app state")));

            ret.Add(host.ShouldPersistState
                .SelectMany(x => driver.SaveState(application.BuildAppState()).Finally(x.Dispose))
                .LoggedCatch(host, Observable.Return(Unit.Default), "Tried to persist app state")
                .Subscribe(_ => host.Log().Info("Persisted application state")));

            ret.Add(Observable.Merge(host.IsResuming, host.IsLaunchingNew)
                .SelectMany(x => driver.LoadState())
                .LoggedCatch(host,
                    Observable.Defer(() => Observable.Return(host.CreateNewAppState())),
                    "Failed to restore app state from storage, creating from scratch")
                .Subscribe(x => host.AppState = x ?? host.CreateNewAppState()));
        }
    }
}
