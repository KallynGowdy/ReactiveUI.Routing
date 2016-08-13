
# ReactiveUI.Routing

[![Build status](https://ci.appveyor.com/api/projects/status/mqqfu1yurd22vgk8/branch/master?svg=true)](https://ci.appveyor.com/project/KallynGowdy/reactiveui-routing/branch/master)

## Goals
- Cross platform logic.
- Strong support for page/fragment/modal navigation.
- Async all the way.
- Support suspend/resume.
- Minimize boilerplate code + Maximize readability.

With these core ideas in mind, here are the conclusions that I have drawn.
See [the proposal](./docs/proposal.md)!

Note that Xamarin.Forms is not currently supported. Support for it is planned.

## Getting Started

1. Add ReactiveUI.Routing to your project. (NuGet not yet available)
2. Inherit `DefaultRoutedAppConfig` in your cross-platform project. This is where you put all of your common registrations, router config, etc.

```csharp
public abstract class CrossPlatformAppConfig : DefaultRoutedAppConfig
{
    public override void RegisterDependencies(IMutableDependencyResolver resolver)
    {
        base.RegisterDependencies(resolver);
        resolver.Register(() => new MyViewModel(), typeof(MyViewModel));
        // ...
    }

    protected override RouterConfig BuildRouterParams()
    {
        // Build your router config here
        return new RouterBuilder().Build();
    }
}
```

3. Inherit your `CrossPlatformAppConfig` for your platform-specific projects, registering platform-specific services.

In Android:

```csharp
public class AndroidAppConfig : CrossPlatformAppConfig
{
    private DefaultAndroidConfig androidConfig;
    
    public AndroidAppConfig(Activity hostActivity, Bundle savedInstanceState) 
    {
        androidConfig = new DefaultAndroidConfig(hostActivity, savedInstanceState);
    } 

    public override void RegisterDependencies(IMutableDependencyResolver resolver)
    {
        base.RegisterDependencies(resolver);
        androidConfig.RegisterDependencies(resolver);
    }

    public override void CloseApp() => androidConfig.CloseApp();
    protected override ISuspensionNotifier BuildSuspensionNotifier() => androidConfig.BuildSuspensionNotifier();
    protected override IObjectStateStore BuildObjectStateStore() => androidConfig.BuildObjectStateStore();
}
```

In iOS:

```csharp
public class iOSAppConfig : RoutedAppConfig
{
    private readonly DefaultiOSConfig iosAppConfig;
    public iOSAppConfig(AppDelegate appDelegate)
    {
        iosAppConfig = new DefaultiOSConfig(appDelegate);
    }
    public override void RegisterDependencies(IMutableDependencyResolver resolver)
    {
        base.RegisterDependencies(resolver);
        iosAppConfig.RegisterDependencies(resolver);
    }
    public override void CloseApp() => iosAppConfig.CloseApp();
    protected override ISuspensionNotifier BuildSuspensionNotifier() => iosAppConfig.BuildSuspensionNotifier();
    protected override IObjectStateStore BuildObjectStateStore() => iosAppConfig.BuildObjectStateStore();
}
```

4. Create a new `RoutedAppHost`, passing in an instance of your platorm-specific app config and call `Start()` on it.

In Android:

```csharp
[Activity(Label = "MainActivity", MainLauncher = true)]
public class MainActivity : SuspendableAcitivity
{
    private IRoutedAppHost host;
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        host = new RoutedAppHost(new AndroidAppConfig(this, savedInstanceState));
        host.Start();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        SuspensionNotifier?.TriggerSuspension();
    }
}
```

In iOS:

```csharp
// DefaultAppDelegate handles the creation of RoutedAppHost for you. 
[Register ("AppDelegate")]
public partial class AppDelegate : DefaultAppDelegate
{
    protected override IRoutedAppConfig BuildAppConfig(UIApplication app, NSDictionary options)
    {
        return new iOSAppConfig(this);
    }
}
```

## Routing Configuration

To configure your routes, override `BuildRouterParams()` in your CrossPlatformAppConfig.
There are two key concepts for the routing configuration:

1. You specify actions for view model types.
2. These actions determine how the view model handles navigation and how it is presented.

You create a `RouterConfig` by using a `RouterBuilder`.
From there, you can specify how actions should be handled for different view models.

```csharp
var builder = new RouterBuilder();

// RouterBuilder.When() is used to specify actions for a view model type.
builder.When<MyViewModel>(routeBuilder => 
{
    // In each call to When(), you need to specify
    // the route actions. You can do that using a
    // route builder lambda.

    // RouteBuilder.Navigate() specifies that 
    // the view model should be treated as a "place" that the 
    // user can go to.
    r.Navigate();

    // RouteBuilder.NavigateFrom() builds on top 
    // of RouteBuilder.Navigate() by navigating backwards
    // until either the ParentViewModel is reached or if the end of the
    // stack is met.
    r.NavigateFrom<ParentViewModel>();

    // RouteBuilder.Present() specifies that the
    // view model should be presented using the registered IPresenter type.
    // This means that the router will get a IPresenter from the Locator
    // and attempt to present the view model with it.
    // Additionally, Present() takes a hint object, which can communicate to the
    // presenter specific detail on displaying the view model.
    r.Present();

    // An alternative to RouteBuilder.Present(),
    // RouteBuilder.PresentPage() specifies that instead of the inspecific IPresenter type,
    // that a IPagePresenter type should be used. Semantically, this means that the 
    // view model will be presented in an Activity or UIViewController.
    r.PresentPage();

    return r;
});

RouterConfig config = builder.Build();
```

## Navigation Patterns

Below are typical use cases for navigation with a router in ReactiveUI.Routing.

### Getting a router

```csharp
var router = Locator.Current.GetService<IRouter>();
```

### Simple Navigation

```csharp
await router.ShowAsync<MyViewModel>();
await router.BackAsync();
await router.ShowDefaultViewModelAsync();
```

### Parameterized Navigation

```csharp
class MyViewModel : ActivatableObject<MyParameterType> 
{
    protected override void InitCoreSync(MyParameterType parameters) 
    {
        // Do stuff with parameters
    }
}

router.ShowAsync<MyViewModel, MyParameterType>(new MyParameterType() {
    Values = "My Values!"
});
```

### Supporting Suspend

```csharp
class MyViewModel : ReActivatableObject<Unit, MyStateType> 
{
    protected override MyStateType SuspendCoreSync() 
    {
        return new MyStateType 
        {
            Value = "My State!"
        };
    }
}
```

### Supporting Resume

```csharp
class MyViewModel : ReActivatableObject<Unit, MyStateType> 
{
    protected override void ResumeCoreSync(MyStateType state) 
    {
        base.ResumeCoreSync(state);
        // Do stuff with state
    }
}
```

## Contributing

If you have thoughts or suggestions, raise an issue that discusses what is on your mind. From there, we can then create a pull request that brings them into the proposal.