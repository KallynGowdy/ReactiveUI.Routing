using System;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents a built transition.
    /// </summary>
    public sealed class Transition : ReActivatableObject<ActivationParams, ObjectState>
    {
        public Transition() : this(null)
        {
        }

        public Transition(IReActivator activator)
        {
            Activator = activator ?? Locator.Current.GetService<IReActivator>() ?? new ReActivator();
        }

        private IReActivator Activator { get; }
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
            if (vm == null) throw new InvalidOperationException($"Cannot initialize transition. The returned value from the Locator.Current.GetService({ActivationParams.Type}) was null, which means that the view model could not be instantiated.");
            ViewModel = vm;
        }

        protected override async Task<ObjectState> SuspendCoreAsync()
        {
            return await Activator.SuspendAsync(ViewModel);
        }

        protected override async Task ResumeCoreAsync(ObjectState storedState, IReActivator reActivator)
        {
            await base.ResumeCoreAsync(storedState, reActivator);
            ViewModel = await reActivator.ResumeAsync(storedState);
        }

        protected override async Task DestroyCoreAsync()
        {
            await base.DestroyAsync();
            await Activator.DeactivateAsync(ViewModel);
        }

        public override string ToString()
        {
            return $"Transition to: {ActivationParams.Type}";
        }
    }
}