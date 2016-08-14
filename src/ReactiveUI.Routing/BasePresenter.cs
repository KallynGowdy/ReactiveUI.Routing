using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a base presenter class that provides functionality like activation for views.
    /// </summary>
    public abstract class BasePresenter : IPresenter, IActivationForViewFetcher
    {
        private readonly Dictionary<ReactiveUI.IActivatable, BehaviorSubject<bool>> activationMap = new Dictionary<ReactiveUI.IActivatable, BehaviorSubject<bool>>();
        protected IViewTypeLocator ViewLocator { get; }

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
            return activation;
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

        protected void NotifyViewActivated(ReactiveUI.IActivatable view) => NotifyActivationForView(view, true);
        protected void NotifyViewDeActivated(ReactiveUI.IActivatable view) => NotifyActivationForView(view, false);
        public abstract Task<IDisposable> PresentAsync(object viewModel, object hint);

        protected Type ResolveViewTypeForViewModelType(Type viewModelType)
        {
            var viewType = ViewLocator.ResolveViewType(viewModelType);
            if (viewType == null)
            {
                throw new InvalidOperationException($"Could not resolve activity for {viewModelType}. Make sure that a IViewFor<{viewModelType}> exists in the current assembly.");
            }
            return viewType;
        }
    }
}
