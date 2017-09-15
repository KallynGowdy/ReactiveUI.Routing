using System;
using System.Reactive.Disposables;
using ReactiveUI.Routing.Presentation;
using ReactiveUI.Routing.UseCases.Common;
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
            throw new System.NotImplementedException();
        }

        public void LoadState(ReactiveAppState state)
        {
            throw new NotImplementedException();
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