using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Splat;

namespace ReactiveUI.Routing.Android
{
    /// <summary>
    /// Defines an activity that implements activation and back button logic for activities.
    /// </summary>
    /// <typeparam name="TViewModel">The type of view model that this activity represents.</typeparam>
    public class RoutableActivity<TViewModel> : SuspendableAcitivity, ICanActivate, ReactiveUI.IActivatable, IViewFor<TViewModel>, INotifyPropertyChanged
        where TViewModel : class
    {
        private readonly Subject<Unit> activated = new Subject<Unit>();
        private readonly Subject<Unit> deactivated = new Subject<Unit>();
        private readonly IRouter router;
        private TViewModel viewModel;

        public event PropertyChangedEventHandler PropertyChanged;
        public IObservable<Unit> Activated =>
            activated.CombineLatest(this.WhenAnyValue(v => v.ViewModel), (unit, vm) => vm)
                .Where(vm => vm != null)
                .Select(v => Unit.Default);
        public IObservable<Unit> Deactivated => deactivated;

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set
            {
                ViewModel = (TViewModel)value;
            }
        }

        public TViewModel ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
                OnPropertyChanged();
            }
        }

        public RoutableActivity() : this(null, null) { }

        public RoutableActivity(IRouter router, SuspensionNotifierHelper supensionNotifier)
            : base(supensionNotifier)
        {
            this.router = router ?? Locator.Current.GetService<IRouter>();
        }

        public override void OnBackPressed()
        {
            router.BackAsync();
        }

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