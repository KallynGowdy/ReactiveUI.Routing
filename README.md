
# ReactiveUI.Routing

[![Build status](https://ci.appveyor.com/api/projects/status/mqqfu1yurd22vgk8/branch/master?svg=true)](https://ci.appveyor.com/project/KallynGowdy/reactiveui-routing/branch/master)

## Goals
- Cross platform logic.
- Declarative Routing
- Strong support for page/fragment/modal navigation.
- Async all the way.
- Support suspend/resume.
- Minimize boilerplate code + Maximize readability.

With these core ideas in mind, here are the conclusions that I have drawn.
See [the proposal](./docs/proposal.md)!

Note that Xamarin.Forms is not currently supported. Support for it is planned.

## Getting Started

### NuGet

Project Feed: `https://ci.appveyor.com/nuget/reactiveui-routing-7x2h1i9nn69m`

### Cross-Platform

1. Add `ReactiveUI.Routing` to each of your projects.
2. Implement `IRegisterDependencies` in your cross-platform project. This is where all of your common registrations go. (router config, services, view models)

```csharp
public class ProjectDependencies : IRegisterDependencies
{
    public void RegisterDependencies(IMutableDependencyResolver resolver) 
    {
        // Build your router configuration. This is explained more in the
        // "Declarative Routing" section, but essentially what it allows
        // you to do is map your view models to presenters and define whether they should
        // affect the navigation stack.  
        var routerConfig = new RouterBuilder().Build();

        resolver.RegisterConstant(routerConfig, typeof(RouterConfig)); 
        resolver.Register(() => new MyViewModel(), typeof(MyViewModel));
        // ...
    }
} 
```

### Xamarin.Forms

If you're using Xamarin.Forms you need to make sure you do the following:

- Add `ReactiveUI.Routing.XamForms` to each of your projects.
- In your cross-platform project, inherit your `App` class from `RoutedApplication`, and implement `BuildAppConfig`.

```csharp
using ReactiveUI.Routing;
using ReactiveUI.Routing.XamForms;

public class App : RoutedApplication
{
    private readonly IRoutedAppConfig platformDependencies;

    // When you are creating your app class from MainActivity or AppDelegate,
    // you can pass in platform dependencies.
    public App(IRoutedAppConfig platformDependencies = null)
    {
        this.platformDependencies = platformDependencies;
        StartHost();
    }

    protected override IRoutedAppConfig BuildAppConfig()
    {
        // Build the IRoutedAppConfig that the application
        // should use.
        // CompositeRoutedAppConfig is a helper class that allows you to 
        // combine multiple IRegisterDependencies or IRoutedAppConfig instances
        // into a single config.
        // Dependencies passed in later will override earlier ones.
        return new CompositeRoutedAppConfig(
            // Include the default dependencies for ReactiveUI.Routing
            new DefaultDependencies(),
            // Include your platform-specific dependencies
            platformDependencies,
            // Include the default ReactiveUI.Routing.XamForms dependencies 
            new DefaultXamFormsDependencies(this),
            // Include your cross platform dependencies
            new ProjectDependencies());
    }
}
```

- For your platform-specific projects, make sure you add references to `ReactiveUI.Routing.Android`, and `ReactiveUI.Routing.iOS` as needed.
- In your Android project, in `MainActivity`, make sure you are passing in the `DefaultAndroidDependencies` to your `App` constructor.
- In your iOS project, in `MainActivity`, make sure you are passing in the `DefaultiOSDepdendencies` to your `App` constructor.
- For UWP or WinPhone, you either need to make sure that you are registering `IObjectStateStore`, as a default `IObjectStateStore` does not currently exist for UWP. 

### Separate UI

If you are not using Xamarin.Forms, you can add `ReactiveUI.Routing` to your application like this: 

1. Add references `ReactiveUI.Routing` to your projects, adding `ReactiveUI.Routing.Android` for Android and `ReactiveUI.Routing.iOS` for iOS.
2. Then create a platform-specific app config:

In Android:

```csharp
using ReactiveUI.Routing;
using ReactiveUI.Routing.Android;

public class AndroidAppConfig : CompositeRoutedAppConfig
{    
    public AndroidAppConfig(Activity hostActivity, Bundle savedInstanceState)
        
        // Provide the dependencies from other projects
        : base(
            new DefaultDependencies(),
            new ProjectDependencies(),
            new DefaultAndroidDependencies(hostActivity, savedInstanceState)
        ) 
    {
    } 

    public override void RegisterDependencies(IMutableDependencyResolver resolver)
    {
        base.RegisterDependencies(resolver);
        // Register platform-specific dependencies
    }
}
```

In iOS:

```csharp
using ReactiveUI.Routing;
using ReactiveUI.Routing.iOS;

public class iOSAppConfig : CompositeRoutedAppConfig
{
    public iOSAppConfig(AppDelegate appDelegate)
        : base(
            new DefaultDependencies(),
            new ProjectDependencies(),
            new DefaultiOSDepdendencies(appDelegate)
        )
    {
    }

    public override void RegisterDependencies(IMutableDependencyResolver resolver)
    {
        base.RegisterDependencies(resolver);
        // register platform-specific depenendencies
    }
}
```

4. Create a new `RoutedAppHost`, passing in an instance of your platorm-specific app config and call `Start()` on it.

In Android:

```csharp
using ReactiveUI.Routing;
using ReactiveUI.Routing.Android;

// SuspendableAcitivity implements ISuspensionNotifier for you
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
using ReactiveUI.Routing;
using ReactiveUI.Routing.iOS;

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

## Declarative Routing

To configure your routes, register a `RouterConfig` object in your dependencies.
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

    // From here, it's not hard to imagine extensions that allow you to do more complicated things.
    // For example, a PresentNotification() extension could be added that uses a presenter specialized to
    // present notifications.

    return r;
});

// A common route might look something like this:
builder.When<MyViewModel>(r => r.Navigate().PresentPage());

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
// you don't have to await these, but it is
// generally a good idea so that your method
// won't continue until the target view model has been
// created and displayed.
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