using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Splat;

namespace ReactiveUI.Routing.Android
{
    /// <summary>
    /// Defines a class that represents a presenter that can present view models inside an android application.
    /// </summary>
    public abstract class AndroidPresenter : BasePresenter
    {
        protected IViewTypeLocator ViewLocator { get; }
        protected Application Application { get; }
        protected Context Context { get; }

        protected AndroidPresenter(Application application = null, Context context = null, IViewTypeLocator viewLocator = null)
        {
            ViewLocator = viewLocator ?? Locator.Current.GetService<IViewTypeLocator>();
            Application = application ?? Locator.Current.GetService<Application>();
            Context = context ?? Locator.Current.GetService<Context>();
        }
    }
}