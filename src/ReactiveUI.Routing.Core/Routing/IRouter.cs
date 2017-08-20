using System.Reactive;

namespace ReactiveUI.Routing.Routing
{
    public interface IRouter
    {
        ReactiveCommand<object, Unit> Navigate { get; }
        ReactiveCommand<object, Unit> NavigateAndReset { get; set; }
    }
}