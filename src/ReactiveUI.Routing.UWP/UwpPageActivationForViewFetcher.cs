using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ReactiveUI.Routing.UWP
{
    public class UwpPageActivationForViewFetcher : IActivationForViewFetcher
    {
        public int GetAffinityForView(Type view)
        {
            if (typeof(Page).IsAssignableFrom(view))
            {
                return 100;
            }

            return 0;
        }

        public IObservable<bool> GetActivationForView(IActivatable view)
        {
            var page = view as Page;

            var viewLoaded =
                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(h => page.Loaded += h, h => page.Loaded -= h)
                    .Select(_ => true);

            var viewUnloaded =
                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(h => page.Unloaded += h, h => page.Unloaded -= h)
                    .Select(_ => false);

            return viewLoaded
                .Merge(viewUnloaded)
                .Select(b =>
                    b
                        ? page.WhenAnyValue(p => p.IsHitTestVisible).SkipWhile(visible => !visible)
                        : Observable.Return(false))
                .Switch()
                .DistinctUntilChanged();
        }
    }
}
