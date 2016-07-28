using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    public static class RouterExtensions
    {
        public static Task ShowAsync<TViewModel>(this IRouter router)
            where TViewModel : IActivatable<Unit>
        {
            return router.ShowAsync<TViewModel, Unit>();
        }

        public static Task ShowAsync<TViewModel, TState>(this IRouter router, TState parameters = default(TState))
            where TViewModel : IActivatable<TState>
            where TState : new()
        {
            if (parameters == null)
            {
                parameters = new TState();
            }
            return router.DispatchAsync(RouterActions.ShowViewModel(typeof(TViewModel), parameters));
        }

        public static Task BackAsync(this IRouter router)
        {
            return router.DispatchAsync(RouterActions.Back());
        }
    }
}
