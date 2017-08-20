using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using Splat;

namespace ReactiveUI.Routing
{
    public static class ViewExtensions
    {
        /// <summary>
        /// Gets an observable that resolves when the given view is deactivated.
        /// </summary>
        /// <param name="view">The view that the deactivation observable should be retrieved for.</param>
        /// <returns></returns>
        public static IObservable<IViewFor> Deactivated(this IViewFor view)
        {
            var activationFetcher = Locator.Current.GetService<IActivationForViewFetcher>();

            return activationFetcher.GetActivationForView(view)
                .FirstAsync(active => !active)
                .Select(active => view);
        }

    }
}
