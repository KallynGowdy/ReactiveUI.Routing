using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Builder
{
    /// <summary>
    /// Defines a class that provides an implementation of <see cref="IRouteBuilder"/>.
    /// </summary>
    public class RouteBuilder : IRouteBuilder
    {
        private readonly List<Func<INavigator, TransitionParams, Task>> navigationActions = new List<Func<INavigator, TransitionParams, Task>>();
        private readonly List<Type> presenters = new List<Type>();

        /// <summary>
        /// Gets the type of the view model.
        /// </summary>
        public Type ViewModelType { get; private set; }

        public IEnumerable<Type> Presenters => presenters;

        public IEnumerable<Func<INavigator, TransitionParams, Task>> NavigationActions => navigationActions;

        public IRouteBuilder SetViewModel(Type viewModelType)
        {
            ViewModelType = viewModelType;
            return this;
        }

        public IRouteBuilder Present(Type presenterType)
        {
            presenters.Add(presenterType);
            return this;
        }

        public IRouteBuilder NavigateBackWhile(Func<object, bool> goBackWhile)
        {
            if (goBackWhile == null) throw new ArgumentNullException(nameof(goBackWhile));
            navigationActions.Add(async (navigator, parameters) =>
            {
                for (var top = navigator.Peek(); top != null && goBackWhile(top.ViewModel); top = navigator.Peek())
                {
                    await navigator.PopAsync();
                }
            });
            return this;
        }

        public IRouteBuilder Navigate()
        {
            navigationActions.Add(async (navigator, parameters) => await navigator.PushAsync(new TransitionParams()
            {
                Type = ViewModelType,
                Params = parameters.Params
            }));
            return this;
        }
    }
}
