# Router

The router is the root of all navigation and presentation. It is in charge of communicating with the `NavigationState` and the `PresentationState` to show view models.
Most notably, the router does things like determine which hints to give the `PresentationState` and manipulates the `NavigationState` to be in the correct state for view models.

```csharp
interface IRouter
{
    Task ShowAsync(Type viewModel, object vmParams);
}
```

The interface for `IRouter` is kept to the absolute minimum to help with testing. Additionally, a couple of extension methods can be added to help consumablility.

```csharp
static class RouterExtensions
{
    public static Task ShowAsync<TViewModel>(this IRouter router, object vmParams) => 
        router.ShowAsync(typeof(TViewModel), vmParams);
    public static Task ShowAsync<TViewModel>(this IRouter router) => 
        router.ShowAsync<TViewModel>(null);
}
```

The actions that a router takes are meant to be composed with a builder. A basic router builder might look like this:

```csharp
interface IRouterBuilder
{
    IRouterBuilder When(Type vmType, Func<IRouteBuilder, IRouteBuilder> buildRoute);
    IRouter Build();
}

interface IRouteBuilder
{
    // Instructs the router to tell the PresentationState to use the given
    // view/presenter type when the route is hit.
    IRouteBuilder Present(Type viewType);

    // Instructs the router to manipulate the NavigationState
    // when the route is hit.
    IRouteBuilder NavigateBack(Func<object, bool> goBackWhile);
    IRouteBuilder Navigate();
}
```

Along with relevent extension methods:

```csharp
static class RouterBuilderExtensions
{
    public static IRouterBuilder When<TViewModel>(this IRouterBuilder builder, Func<IRouteBuilder, IRouteBuilder> buildRoute) =>
            builder.When(typeof(TViewModel), buildRoute);
    public static IRouteBuilder When<TViewModel>(this IRouterBuilder builder) => 
            builder.When<TViewModel>(null);
}

static class RouteBuilderExtensions
{
    public static IRouteBuilder Present<TView>(this IRouteBuilder route) =>
            route.Present(typeof(TView)); 
    public static IRouteBuilder NavigateFrom<TParentViewModel>(this IRouteBuilder route) =>
            route.NavigateBack(vm => !(vm is TParentViewModel));
}
```