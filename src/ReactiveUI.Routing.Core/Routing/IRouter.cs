using System.Reactive;

namespace ReactiveUI.Routing.Core.Routing
{
    public interface IRouter
    {
        ReactiveCommand<object, Unit> Navigate { get; }
        ReactiveCommand<object, Unit> NavigateAndReset { get; set; }
    }
}