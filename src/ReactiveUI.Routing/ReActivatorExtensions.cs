using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    public static class ReActivatorExtensions
    {
        public static async Task<T> ResumeAsync<T>(this IReActivator activator, ObjectState state)
        {
            return (T)await activator.ResumeAsync(state);
        }

        public static async Task<T> ResumeAsync<T, TParams, TState>(this IReActivator activator, TParams parameters,
            TState state)
            where T : IReActivatable<TParams, TState>
            where TParams : new()
            where TState : new()
        {
            return (T)await activator.ResumeAsync(new ObjectState()
            {
                Params = new ActivationParams()
                {
                    Type = typeof(T),
                    Params = parameters
                },
                State = state
            });
        }

    }
}
