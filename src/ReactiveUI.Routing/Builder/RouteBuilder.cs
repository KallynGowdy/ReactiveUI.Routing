using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Actions;

namespace ReactiveUI.Routing.Builder
{
    /// <summary>
    /// Defines a class that provides an implementation of <see cref="IRouteBuilder"/>.
    /// </summary>
    public class RouteBuilder : IRouteBuilder
    {
        private readonly List<IRouteAction> actions = new List<IRouteAction>();

        /// <summary>
        /// Gets the type of the view model.
        /// </summary>
        public Type ViewModelType { get; private set; }

        public IRouteBuilder SetViewModel(Type viewModelType)
        {
            ViewModelType = viewModelType;
            return this;
        }

        public IRouteBuilder Present(Type presenterType)
        {
            actions.Add(RouteActions.Present(presenterType));
            return this;
        }

        public IRouteBuilder NavigateBackWhile(Func<Transition, bool> goBackWhile)
        {
            if (goBackWhile == null) throw new ArgumentNullException(nameof(goBackWhile));
            actions.Add(RouteActions.NavigateBackWhile(goBackWhile));
            return this;
        }

        public IRouteBuilder Navigate()
        {
            actions.Add(RouteActions.Navigate());
            return this;
        }

        public RouteActions Build()
        {
            return new RouteActions()
            {
                Actions = actions.ToArray(),
                ViewModelType = ViewModelType
            };
        }
    }
}
