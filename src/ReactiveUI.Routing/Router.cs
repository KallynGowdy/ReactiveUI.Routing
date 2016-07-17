using System;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    public class Router : ReActivatableObject<RouterParams, RouterState>, IRouter
    {
        public INavigator Navigator { get; }
        public IPresenter Presenter { get; }

        public Router(): this(null, null)
        {
        }

        public Router(INavigator navigator, IPresenter presenter)
        {
            this.Navigator = navigator ?? Locator.Current.GetService<INavigator>();
            this.Presenter = presenter ?? Locator.Current.GetService<IPresenter>();
            if (this.Navigator == null) throw new InvalidOperationException("When creating a router, a INavigator object must either be provided or locatable via Locator.Current.GetService<INavigator>()");
            if(this.Presenter == null) throw new InvalidOperationException("When creating a router, a IPresenter object must either be provided or locatable via Locator.Current.GetService<IPresenter>()");
        }

        public async Task ShowAsync(Type viewModel, object vmParams)
        {
            CheckInit();
        }

        private void CheckInit()
        {
            if(!Initialized) throw new InvalidOperationException("The router must be initialized before use.");
        }
    }
}