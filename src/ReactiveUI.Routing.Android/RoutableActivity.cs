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
    public class RoutableActivity<TViewModel> : SuspendableAcitivity, IViewFor<TViewModel>, INotifyPropertyChanged
        where TViewModel : class
    {
        private readonly IRouter router;
        private TViewModel viewModel;
        public event PropertyChangedEventHandler PropertyChanged;

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

        public RoutableActivity(IRouter router, SuspensionNotifierHelper suspensionNotifier)
            : base(suspensionNotifier)
        {
            this.router = router ?? Locator.Current.GetService<IRouter>();
        }

        public override void OnBackPressed()
        {
            router.BackAsync();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}