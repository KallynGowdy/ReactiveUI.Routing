using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using Splat;

namespace ReactiveUI.Routing
{
    public static class SuspensionHostExtensions
    {

        public static IDisposable SetupPersistence(this ISuspensionHost host, Func<object> getAppState, ISuspensionDriver driver = null)
        {
            var ret = new CompositeDisposable();
            driver = driver ?? Locator.Current.GetService<ISuspensionDriver>();

            host.CreateNewAppState = getAppState;

            ret.Add(host.ShouldInvalidateState
                .SelectMany(_ => driver.InvalidateState())
                .LoggedCatch(host, Observable.Return(Unit.Default), "Tried to invalidate app state")
                .Subscribe(_ => host.Log().Info("Invalidated app state")));

            ret.Add(host.ShouldPersistState
                .SelectMany(x => driver.SaveState(getAppState()).Finally(x.Dispose))
                .LoggedCatch(host, Observable.Return(Unit.Default), "Tried to persist app state")
                .Subscribe(_ => host.Log().Info("Persisted application state")));

            ret.Add(Observable.Merge(host.IsResuming, host.IsLaunchingNew)
                .SelectMany(x => driver.LoadState())
                .LoggedCatch(host,
                    Observable.Defer(() => Observable.Return(host.CreateNewAppState())),
                    "Failed to restore app state from storage, creating from scratch")
                .Subscribe(x => host.AppState = x ?? host.CreateNewAppState()));

            return ret;
        }

    }
}
