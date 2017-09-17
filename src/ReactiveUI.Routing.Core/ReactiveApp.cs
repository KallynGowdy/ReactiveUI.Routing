using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing
{
    public class ReactiveApp : IReactiveApp
    {
        public IReactiveRouter Router { get; }
        public IAppPresenter Presenter { get; }
        public ISuspensionHost SuspensionHost { get; }
        public ISuspensionDriver SuspensionDriver { get; }
        public IMutableDependencyResolver Locator { get; }

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public ReactiveApp(
            IReactiveRouter router,
            IAppPresenter presenter,
            ISuspensionHost suspensionHost,
            ISuspensionDriver suspensionDriver,
            IMutableDependencyResolver locator)
        {
            this.Router = router;
            this.Presenter = presenter;
            this.SuspensionHost = suspensionHost;
            this.SuspensionDriver = suspensionDriver;
            this.Locator = locator;
        }

        public ReactiveAppState BuildAppState()
        {
            return new ReactiveAppState()
            {
                PresentationState = Presenter.GetPresentationState()
            };
        }

        public IObservable<Unit> LoadState(ReactiveAppState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            return new[]
            {
                state.PresentationState == null ? null : Presenter.LoadState(state.PresentationState),

                Observable.Return(Unit.Default)
            }.Where(observable => observable != null)
             .Zip()
             .Select(_ => Unit.Default);
        }

        public void RegisterDisposable(IDisposable disposable)
        {
            if (disposable == null) throw new ArgumentNullException(nameof(disposable));
            this.disposable.Add(disposable);
        }

        public void Dispose()
        {
            disposable.Clear();
        }
    }
}