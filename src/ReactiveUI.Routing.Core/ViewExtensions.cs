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
        /// <param name="activationForViewFetcher">The activation fetcher that should be used.</param>
        /// <returns></returns>
        public static IObservable<IViewFor> Deactivated(this IViewFor view, IActivationForViewFetcher activationForViewFetcher = null)
        {
            var activationFetcher = activationForViewFetcher
                ?? Locator.Current.GetService<IActivationForViewFetcher>()
                ?? throw new ArgumentNullException(nameof(activationForViewFetcher), "An IActivationForViewFetcher must be either provided by argument or registered in Locator.Current");

            return activationFetcher.GetActivationForView(view)
                .FirstAsync(active => !active)
                .Select(active => view);
        }

    }
}
