using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Actions;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that contains common core functionality for routed apps.
    /// </summary>
    public class RoutedAppHost : IRoutedAppHost, IEnableLogger
    {
        private readonly IRoutedAppConfig config;
        private bool started = false;

        public RoutedAppHost(IRoutedAppConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            this.config = config;
        }

        public void Start()
        {
            StartAsync().Wait();
        }

        public virtual async Task StartAsync()
        {
            RegisterDependencies();
            var stateStore = GetService<IObjectStateStore>();
            var notifier = GetService<ISuspensionNotifier>();
            var routerParams = GetService<RouterConfig>();
            var router = GetService<IRouter>();

            SetupSubscriptions(router, stateStore, notifier);

            var routerState = await GetRouterState(stateStore);
            await ResumeRouterAsync(router, routerParams, routerState, stateStore);
            await router.ShowDefaultViewModelAsync();
            
            started = true;
        }

        private void SetupSubscriptions(IRouter router, IObjectStateStore stateStore, ISuspensionNotifier notifier)
        {
            if (!started)
            {
                notifier.OnSaveState.Throttle(TimeSpan.FromMilliseconds(10))
                    .Do(async u => await SaveRouterStateAsync(router, stateStore))
                    .Subscribe();

                router.CloseApp
                    .Do(u => config.CloseApp())
                    .Subscribe();
                notifier.OnSuspend
                    .FirstAsync()
                    .Do(async u => { await ActivationHelpers.DestroyObjectAsync(router); })
                    .Subscribe();
            }
        }

        private void RegisterDependencies()
        {
            if (!started)
            {
                config.RegisterDependencies(Locator.CurrentMutable);
            }
        }

        private static async Task SaveRouterStateAsync(IRouter router, IObjectStateStore stateStore)
        {
            var state = await ActivationHelpers.GetObjectStateAsync(router);
            await stateStore.SaveStateAsync(state);
        }

        private async Task ResumeRouterAsync(IRouter router, RouterConfig routerConfig, RouterState routerState, IObjectStateStore stateStore)
        {
            await ActivationHelpers.InitObjectAsync(router, routerConfig);
            try
            {
                if (routerState != null)
                {
                    await ActivationHelpers.ResumeObjectStateAsync(router, routerState);
                }
            }
            catch (Exception e)
            {
                this.Log().WarnException("Could not resume the router state.", e);
                await InvalidateStateAsync(stateStore);
            }
        }

        private async Task<RouterState> GetRouterState(IObjectStateStore stateStore)
        {
            ObjectState existingState = null;
            try
            {
                existingState = await stateStore.LoadStateAsync();
            }
            catch (Exception e)
            {
                this.Log().WarnException("Could not load state from state store.", e);
                await InvalidateStateAsync(stateStore);
            }
            RouterState routerState = null;
            try
            {
                routerState = (RouterState)existingState?.State;
            }
            catch (InvalidCastException e)
            {
                this.Log().WarnException($"Could not cast loaded state to {nameof(RouterState)}. Was a breaking change made to the app's stored data model?", e);
                await InvalidateStateAsync(stateStore);
            }
            return routerState;
        }

        private static async Task InvalidateStateAsync(IObjectStateStore stateStore)
        {
            await stateStore.SaveStateAsync(null);
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
