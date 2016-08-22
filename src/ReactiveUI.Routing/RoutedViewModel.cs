using System;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a base view model that is able to utilize a <see cref="IRouter"/> to navigate/present other
    /// view models.
    /// </summary>
    /// <typeparam name="TParams"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public class RoutedViewModel<TParams, TState> : ReActivatableObject<TParams, TState>
        where TParams : new()
        where TState : new()
    {
        public RoutedViewModel(IRouter router)
        {
            Router = router ?? Locator.Current.GetService<IRouter>();
            if(Router == null) throw new ArgumentNullException(nameof(router));
        }

        /// <summary>
        /// Gets the router that this view model is using.
        /// </summary>
        public IRouter Router { get; }
    }
}
