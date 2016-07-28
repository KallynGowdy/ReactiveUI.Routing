using System;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an abstract class that implements a base ReActivator.
    /// </summary>
    public abstract class BaseReActivator : IReActivator
    {
        protected IActivator Activator { get; }

        protected BaseReActivator() : this(null) { }
        protected BaseReActivator(IActivator activator)
        {
            Activator = activator ?? Locator.Current.GetService<IActivator>();
        }

        public async Task<ObjectState> SuspendAsync(object activated)
        {
            if (activated == null) throw new ArgumentNullException(nameof(activated));
            var activatable = activated as IActivatable;
            if (activatable != null)
            {
                return await SuspendActivatableAsync(activatable);
            }
            return new ObjectState()
            {
                Params = new ActivationParams()
                {
                    Type = activated.GetType(),
                    Params = null
                },
                State = null
            };
        }

        private async Task<ObjectState> SuspendActivatableAsync(IActivatable activatable)
        {
            var activationParams = GetActivationParams(activatable);
            object state = null;
            var reactivatable = activatable as IReActivatable;
            if (reactivatable != null)
            {
                state = await reactivatable.SuspendAsync();
            }
            var ret = new ObjectState()
            {
                Params = activationParams,
                State = state
            };
            await DeactivateAsync(activatable);
            return ret;
        }

        private ActivationParams GetActivationParams(IActivatable activatable)
        {
            return new ActivationParams()
            {
                Params = activatable.InitParams,
                Type = activatable.GetType()
            };
        }

        public async Task<object> ResumeAsync(ObjectState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            var obj = await ActivateAsync(state.Params);
            var reactivatable = obj as IReActivatable;
            if (reactivatable != null && state.State != null)
            {
                await reactivatable.ResumeAsync(state.State, this);
            }
            return obj;
        }

        public Task<object> ActivateAsync(ActivationParams parameters)
        {
            return Activator.ActivateAsync(parameters);
        }

        public Task DeactivateAsync(object activated)
        {
            return Activator.DeactivateAsync(activated);
        }
    }
}