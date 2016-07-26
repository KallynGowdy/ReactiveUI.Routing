using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Extension methods for <see cref="IActivator"/> objects.
    /// </summary>
    public static class ActivatorExtensions
    {
        public static Task<T> ActivateAsync<T>(this IActivator activator, Unit parameters)
            where T : IActivatable<Unit>
        {
            return activator.ActivateAsync<T, Unit>(parameters);
        }

        public static async Task<T> ActivateAsync<T, TParams>(this IActivator activator, TParams parameters)
            where T : IActivatable<TParams>
            where TParams : new()
        {
            return (T) await activator.ActivateAsync(new ActivationParams()
            {
                Type = typeof(T),
                Params = parameters
            });
        }

    }
}

