using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using Foundation;
using Splat;
using UIKit;

namespace ReactiveUI.Routing.iOS
{
    public abstract class DefaultAppDelegate : UIApplicationDelegate, ISuspensionNotifier
    {
        private readonly IRoutedAppConfig appConfig;
        private readonly Subject<Unit> onSaveState = new Subject<Unit>();
        private readonly Subject<Unit> onSuspend = new Subject<Unit>();
        readonly Lazy<NSCoderObjectStateStore> stateStore = new Lazy<NSCoderObjectStateStore>(() => Locator.Current.GetService<NSCoderObjectStateStore>());
        public IObservable<Unit> OnSaveState => onSaveState;
        public IObservable<Unit> OnSuspend => onSuspend;

        public override bool ShouldSaveApplicationState(UIApplication application, NSCoder coder)
        {
            stateStore.Value.Coder = coder;
            return base.ShouldSaveApplicationState(application, coder);
        }

        public override bool ShouldRestoreApplicationState(UIApplication application, NSCoder coder)
        {
            stateStore.Value.Coder = coder;
            return base.ShouldRestoreApplicationState(application, coder);
        }

        public override void OnResignActivation(UIApplication application)
        {
            onSuspend.OnNext(Unit.Default);
            base.OnResignActivation(application);
        }

        protected abstract IRoutedAppConfig BuildAppConfig(UIApplication app, NSDictionary options);

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            base.FinishedLaunching(app, options);
            var host = new RoutedAppHost(BuildAppConfig(app, options));
            host.Start();
            return true;
        }
    }
}
