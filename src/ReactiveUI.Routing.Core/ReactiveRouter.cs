using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI.Routing.Presentation;

namespace ReactiveUI.Routing
{
    public class ReactiveRouter : IReactiveRouter
    {
        private Stack<NavigationRequest> navigationStack = new Stack<NavigationRequest>();

        public Interaction<PresenterRequest, PresenterResponse> PresentationRequested { get; } = new Interaction<PresenterRequest, PresenterResponse>();
        public IEnumerable<NavigationRequest> NavigationStack => navigationStack;

        public IObservable<Unit> Navigate(NavigationRequest request)
        {
            return Observable.Defer(() =>
            {
                return Observable.FromAsync(async () =>
                {
                    if (request is BackNavigationRequest)
                    {
                        navigationStack.Pop();
                        await PresentationRequested.Handle(navigationStack.Peek().PresenterRequest);
                    }
                    else if (request is ResetNavigationRequest)
                    {
                        navigationStack.Clear();
                    }
                    else if (request is CombinedNavigationRequest combined)
                    {
                        foreach (var r in combined)
                        {
                            await Navigate(r);
                        }
                    }
                    else
                    {
                        var result = await PresentationRequested.Handle(request.PresenterRequest);
                        navigationStack.Push(request);
                    }
                });
            });
        }

        public IObservable<bool> CanNavigate(NavigationRequest request)
        {
            return Observable.Return(false);
        }
    }
}