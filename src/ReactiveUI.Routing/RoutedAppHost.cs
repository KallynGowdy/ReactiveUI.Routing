using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Actions;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that contains common core functionality for routed apps.
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    public class RoutedAppHost : IRoutedAppHost
    {
        public IRoutedAppConfig Config { get; }

        public RoutedAppHost(IRoutedAppConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            Config = config;
        }

        public void Start()
        {
            var task = Task.Run(StartAsync);
            if (ModeDetector.InUnitTestRunner())
            {
                task.Wait();
            }
        }

        public virtual async Task StartAsync()
        {
            Config.RegisterDependencies(Locator.CurrentMutable);
            var stateStore = GetService<IObjectStateStore>();
            var notifier = GetService<ISuspensionNotifier>();
            var routerParams = GetService<RouterParams>();
            var activator = GetService<IReActivator>();
            var router = GetService<IRouter>();
            var routerState = await GetRouterState(stateStore);
            await ResumeRouterAsync(router, activator, routerParams, routerState, stateStore);
            await router.ShowDefaultViewModelAsync();

            notifier.OnSaveState
                .Do(async u =>
                {
                    var state = await activator.SuspendAsync(router);
                    await stateStore.SaveStateAsync(state);
                })
                .Subscribe();

            notifier.OnSuspend
                .FirstAsync()
                .Do(async u =>
                {
                    await activator.DeactivateAsync(router);
                })
                .Subscribe();
        }

        private static async Task ResumeRouterAsync(IRouter router, IReActivator activator, RouterParams routerParams, RouterState routerState, IObjectStateStore stateStore)
        {
            await router.InitAsync(routerParams);
            try
            {
                if (routerState != null)
                {
                    await router.ResumeAsync(routerState, activator);
                }
            }
            catch
            {
                await stateStore.SaveStateAsync(null);
                // Make sure that the router gets started
                // TODO: Log exceptions
            }
        }

        private static async Task<RouterState> GetRouterState(IObjectStateStore stateStore)
        {
            ObjectState existingState = null;
            try
            {
                existingState = await stateStore.LoadStateAsync();
            }
            catch
            {
                await stateStore.SaveStateAsync(null);
            }
            RouterState routerState = null;
            try
            {
                routerState = (RouterState)existingState?.State;
            }
            catch (InvalidCastException)
            {
                await stateStore.SaveStateAsync(null);
            }
            return routerState;
        }

        private T GetService<T>()
        {
            var service = Locator.Current.GetService<T>();
            if (service == null)
            {
                throw new InvalidOperationException($"The {nameof(IRoutedAppConfig)} must register a {typeof(T)} service so that the router can be started.");
            }
            return service;
        }
    }
}
