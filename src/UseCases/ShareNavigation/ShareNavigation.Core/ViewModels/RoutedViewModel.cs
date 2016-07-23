using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing;
using Splat;

namespace ShareNavigation.ViewModels
{
    public class RoutedViewModel<TParams, TState> : ReActivatableObject<TParams, TState>
        where TParams : new()
        where TState : new()
    {
        public RoutedViewModel(IRouter router)
        {
            Router = router ?? Locator.Current.GetService<IRouter>();
        }

        public IRouter Router { get; }
    }
}
