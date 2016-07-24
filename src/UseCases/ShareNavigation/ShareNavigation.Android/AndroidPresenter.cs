using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using ReactiveUI.Routing;
using Splat;

namespace ShareNavigation
{
    /// <summary>
    /// Defines a class that represents a presenter that can present view models inside an android application.
    /// </summary>
    public abstract class AndroidPresenter : IPresenter
    {
        protected IViewTypeLocator ViewLocator { get; }
        protected Application Context { get; }

        protected AndroidPresenter(Application context, IViewTypeLocator viewLocator = null)
        {
            ViewLocator = viewLocator ?? Locator.Current.GetService<IViewTypeLocator>();
            this.Context = context ?? Locator.Current.GetService<Application>();
        }

        public abstract Task<IDisposable> PresentAsync(object viewModel, object hint);
    }
}