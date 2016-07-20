using System;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a built transition.
    /// </summary>
    public sealed class Transition : ReActivatableObject<ActivationParams, TransitionState>
    {
        public Transition() : this(null)
        {
        }

        public Transition(IActivator activator)
        {
            Activator = activator ?? Locator.Current.GetService<IActivator>() ?? new LocatorActivator();
        }

        private IActivator Activator { get; }
        private ActivationParams ActivationParams { get; set; }

        /// <summary>
        /// Gets the view model that was created with this transition.
        /// </summary>
        public object ViewModel { get; set; }

        protected override async Task InitCoreAsync(ActivationParams parameters)
        {
            await base.InitCoreAsync(parameters);
            ActivationParams = parameters;
            var vm = await Activator.ActivateAsync(ActivationParams);
            if (vm == null) throw new InvalidOperationException($"Cannot initialize transition. The returned value from Locator.Current.GetService({ActivationParams.Type}) was null, which means that the view model could not be instantiated.");
            ViewModel = vm;
        }

        protected override async Task DestroyCoreAsync()
        {
            await base.DestroyAsync();
            await Activator.DeactivateAsync(ViewModel);
        }
    }
}