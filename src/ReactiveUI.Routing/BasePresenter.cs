using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a base presenter class that provides common presenter functionality like activation for views.
    /// </summary>
    public abstract class BasePresenter : IPresenter, IActivationForViewFetcher
    {
        private readonly Dictionary<ReactiveUI.IActivatable, BehaviorSubject<bool>> activationMap = new Dictionary<ReactiveUI.IActivatable, BehaviorSubject<bool>>();
        protected IViewTypeLocator ViewLocator { get; }

        /// <summary>
        /// Creates a new <see cref="BasePresenter"/>.
        /// </summary>
        /// <param name="viewLocator">The <see cref="IViewTypeLocator"/> that should be used to resolve view type information for view models. If null, then the current <see cref="Locator"/> will be used to resolve a value.</param>
        protected BasePresenter(IViewTypeLocator viewLocator = null)
        {
            ViewLocator = viewLocator ?? Locator.Current.GetService<IViewTypeLocator>();
        }

        public abstract int GetAffinityForView(Type view);

        public IObservable<bool> GetActivationForView(ReactiveUI.IActivatable view)
        {
            BehaviorSubject<bool> activation;
            if (activationMap.TryGetValue(view, out activation)) return activation;
            activation = new BehaviorSubject<bool>(false);
            activationMap.Add(view, activation);
            return activation.DistinctUntilChanged();
        }

        private void NotifyActivationForView(ReactiveUI.IActivatable view, bool activated)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            BehaviorSubject<bool> activation;
            if (activationMap.TryGetValue(view, out activation))
                activation.OnNext(activated);
            else
            {
                activation = new BehaviorSubject<bool>(activated);
                activationMap.Add(view, activation);
            }
        }

        /// <summary>
        /// Notifies that the given view was activated. This can be used so that views that utilize <see cref="ViewForMixins.WhenActivated(ISupportsActivation,Func{IEnumerable{IDisposable}})"/> will work as expected.
        /// </summary>
        /// <param name="view">The view that the activation should be notified for.</param>
        protected void NotifyViewActivated(ReactiveUI.IActivatable view) => NotifyActivationForView(view, true);

        /// <summary>
        /// Notifies that the given view was de-activated. This can be used so that views that utilize <see cref="ViewForMixins.WhenActivated(ISupportsActivation,Func{IEnumerable{IDisposable}})"/> will dispose registrations as expected.
        /// </summary>
        /// <param name="view"></param>
        protected void NotifyViewDeActivated(ReactiveUI.IActivatable view) => NotifyActivationForView(view, false);
        public abstract Task<IDisposable> PresentAsync(object viewModel, object hint);

        /// <summary>
        /// <para>
        /// Resolves the <see cref="Type"/> via the <see cref="ViewLocator"/> that should be instantiated as the view for the 
        /// given <paramref name="viewModelType"/>. 
        /// </para>
        /// <para>
        /// Throws a <see cref="InvalidOperationException"/> if a view for the view model could not be found.
        /// </para>
        /// </summary>
        /// <param name="viewModelType">The type of the view model that the view should be resolved for.</param>
        /// <returns>Returns the type of the view.</returns>
        protected Type ResolveViewTypeForViewModelType(Type viewModelType)
        {
            var viewType = ViewLocator.ResolveViewType(viewModelType);
            if (viewType == null)
            {
                throw new InvalidOperationException($"Could not resolve activity for {viewModelType}. Make sure that a IViewFor<{viewModelType}> exists in the current assembly.");
            }
            return viewType;
        }

        /// <summary>
        /// <para>
        /// Instantiates the given <paramref name="viewType"/> via the current <see cref="Locator"/>.
        /// </para>
        /// <para>
        /// Throws an <see cref="InvalidOperationException"/> if the view could not be created.
        /// </para>
        /// </summary>
        /// <param name="viewType">The type of the view that should be created.</param>
        /// <returns>Returns the created view.</returns>
        protected IViewFor CreateViewFromType(Type viewType)
        {
            var view = (IViewFor)Locator.Current.GetService(viewType);
            if (view == null)
            {
                throw new InvalidOperationException($"Could not resolve view for type {viewType}. Make sure that a {viewType} has been registered with the Locator via {nameof(IRegisterDependencies.RegisterDependencies)}.");
            }
            return view;
        }
    }
}
