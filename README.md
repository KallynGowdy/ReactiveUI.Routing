
# ReactiveUI.Routing

Master

[![Build status](https://ci.appveyor.com/api/projects/status/72dyimy7kn47gr30/branch/master?svg=true)](https://ci.appveyor.com/project/KallynGowdy/reactiveui-routing/branch/master)

## Goals
- Cross platform logic.
- Strong support for page/fragment/modal navigation.
- Async all the way.
- Support suspend/resume.
- Minimize boilerplate code + Maximize readability.

With these core ideas in mind, here are the conclusions that I have drawn.
See [the proposal](./docs/proposal.md)!

## Getting Started

### Running your app

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
        return new RouterBuilder();
    }
}
```

3. Inherit your `CrossPlatformAppConfig` for your platform-specific projects, registering platform-specific services.
4. Create a new `RoutedAppHost`, passing in an instance of your platorm-specific app config.
5. Call `Start()` or `StartAsync()` on the new `RoutedAppHost` when the application is ready to start.

### Routing Configuration

1. Configure your router.

```csharp
// CrossPlatformAppConfig.cs
protected override RouterConfig BuildRouterParams()
{
    return new RouterBuilder()
        // Specifies that when ShowAsync<MyViewModel>() is
        // called on the router, that it should be navigated to
        // and presented.
        .When<MyViewModel>(r => r.Navigate().Present());
}
```

2. Obtain an instance to a `IRouter`.

```csharp
var router = Locator.Current.GetService<IRouter>();
```

## ViewModel Patterns

### Simple Navigation

```csharp
class MyViewModel : ActivatableObject<Unit> {}

router.ShowAsync<MyViewModel>();
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

### Suspend

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

### Resume

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