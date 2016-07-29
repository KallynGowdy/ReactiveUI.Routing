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
            var existingState = await stateStore.LoadStateAsync();
            var activator = GetService<IReActivator>();
            var routerState = await GetRouterState(existingState, stateStore);
            var router = await ResumeRouterAsync(activator, routerParams, routerState, stateStore);
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

        private static async Task<IRouter> ResumeRouterAsync(IReActivator activator, RouterParams routerParams, RouterState routerState, IObjectStateStore stateStore)
        {
            try
            {
                return await activator.ResumeAsync<IRouter, RouterParams, RouterState>(routerParams, routerState);
            }
            catch (Exception e)
            {
                await stateStore.SaveStateAsync(null);
                // Make sure that the router gets started
                // TODO: Log exceptions
                return await activator.ActivateAsync<IRouter, RouterParams>(routerParams);
            }
        }

        private static async Task<RouterState> GetRouterState(ObjectState existingState, IObjectStateStore stateStore)
        {
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
