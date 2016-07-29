using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using ReactiveUI.Routing;
using Splat;
using IActivatable = ReactiveUI.IActivatable;

namespace ShareNavigation
{
    public class RoutableActivity<T> : Activity, ICanActivate, IActivatable, IViewFor<T>, INotifyPropertyChanged
        where T : class
    {
        private readonly Lazy<SuspensionNotifierHelper> suspensionNotifier;
        private Subject<Unit> activated = new Subject<Unit>();
        private Subject<Unit> deactivated = new Subject<Unit>();
        protected SuspensionNotifierHelper SuspensionNotifier => suspensionNotifier.Value;
        private readonly IRouter router;
        private T viewModel;

        public RoutableActivity() : this(null, null) { }
        public RoutableActivity(IRouter router, SuspensionNotifierHelper supensionNotifier)
        {
            this.suspensionNotifier = new Lazy<SuspensionNotifierHelper>(() =>
                supensionNotifier ?? Locator.Current.GetService<SuspensionNotifierHelper>());
            this.router = router ?? Locator.Current.GetService<IRouter>();
        }

        public override void OnBackPressed()
        {
            router.BackAsync();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            SuspensionNotifier.TriggerSaveState();
            base.OnSaveInstanceState(outState);
        }

        public IObservable<Unit> Activated =>
            activated.CombineLatest(this.WhenAnyValue(v => v.ViewModel), (unit, viewModel) => viewModel)
                .Where(viewModel => viewModel != null)
            .Select(v => Unit.Default);
        public IObservable<Unit> Deactivated => deactivated;

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set
            {
                ViewModel = (T)value;
            }
        }

        public T ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPause()
        {
            base.OnPause();
            deactivated.OnNext(Unit.Default);
        }

        protected override void OnResume()
        {
            base.OnResume();
            activated.OnNext(Unit.Default);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}