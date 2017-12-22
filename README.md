
# ReactiveUI.Routing

[![Build status](https://ci.appveyor.com/api/projects/status/mqqfu1yurd22vgk8/branch/master?svg=true)](https://ci.appveyor.com/project/KallynGowdy/reactiveui-routing/branch/master)

## Goals
- Cross platform logic.
- Simple presenter pattern.
- Strong support for page/fragment/modal navigation.
- Async all the way.
- Support suspend/resume.
- Minimize boilerplate code + Maximize readability.

> Note that this library is currently a **work-in-progress**. A version 1.0.0 release will be published when it is finished.

## Getting Started

### NuGet

Project Feed: `https://ci.appveyor.com/nuget/reactiveui-routing-7x2h1i9nn69m`

### Setup

1. Add the `KallynGowdy.ReactiveUI.Routing` NuGet package (from the project feed above) to each of your projects.
2. Create a `ReactiveUI.Routing.IReactiveApp` in your app project (i.e. Android/WPF/UWP/etc) by using the `ReactiveAppBuilder` class.
    1. Use the `Add()` methods to register custom `IReactiveAppDependency` classes that register dependencies for your project. (Or just register them to `Splat.Locator.CurrentMutable`)
    2. Call `ConfigureXYZ()` to add a configuration for your specific platform.
    3. Call `Build()`.

3. Call `PagePresenter.RegisterHost()` to configure view hosts for view models.
4. Dependency inject `IReactiveRouter` into your view models.
5. Call `IReactiveRouter.Navigate()` to present view models.
6. Profit!

UWP Example:
```csharp
// ### App.xaml.cs
protected override void OnLaunched(LaunchActivatedEventArgs e)
{
  var app = new ReactiveAppBuilder()
    .AddReactiveRouting()
    .Add(new MyCommonDependencies())
    .Add(new MyPlatformSpecificDependencies())
    .ConfigureUwp((Application)this, e)
    .Build();
    
  var content = Window.Current.Content as Frame;
  if (content == null)
  {
    content = new Frame();
    Window.Current.Content = content;
  }

  PagePresenter.RegisterHost(content);
  
  // Other Stuff...
}

// ### MyPlatformSpecificDependencies.cs

// Inherit from IReactiveAppDependency so that we can register views
// into the IoC container
public class MyPlatformSpecificDependencies : IReactiveAppDependency
{
  public void Apply(IMutableDependencyResolver resolver)
  {
    // Register all of our views here as resolutions for the respective view models.
    // When navigating to MyViewModel, the IReactiveRouter will request presentation for it
    // through the IAppPresenter, which in turn will attempt to resolve a dependency for 
    // IViewFor<MyViewModel>, which will return our view.
    resolver.Register(() => new MyView(), typeof(IViewFor<MyViewModel>));
    
    // If you use the code below, you'll want to define a view for OtherViewModel and 
    // add a corresponding entry for it here.
  }
}

// ### MyViewModel.cs

// Inject IReactiveRouter into the constructor
public MyViewModel(IReactiveRouter router = null)
{
  // Or locate it using the service locator
  var appRouter = router ?? Locator.Current.GetService<IReactiveRouter>();
  
  MyCommand = ReactiveCommand.CreateAsyncTask(async () => {
    
    // Navigate to OtherViewModel when MyCommand is executed.
    // This will issue a presentation request for OtherViewModel
    // as well as add it to the navigation stack, so that history can be saved.
    await appRouter.Navigate(NavigationRequest.Forward(new OtherViewModel()));
  });
}

// ### MyView.cs

// MyView inherits from IViewFor<MyViewModel>, which means that it
// can present a MyViewModel object.
public partial class MyView : Page, IViewFor<MyViewModel>
{
  public MyView()
  {
    InitializeComponent();
    this.WhenActivated(d => {
    
      // Bind MyButton's click to MyCommand
      this.BindCommand(ViewModel, vm => vm.MyCommand, view => view.MyButton)
          .DisposeWith(d);
    });
  }
}

```

## Contributing

If you have thoughts or suggestions, raise an issue that discusses what is on your mind. From there, we can then create a pull request that brings them into the proposal.
