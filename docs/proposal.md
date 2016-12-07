# Proposal

The current state of routing in ReactiveUI can be a little difficult to work with. 
You have many options, but none of them are as straightforward as they could be, and have some pitfalls.
The goal of this library is to provide a simple cross-platform solution for a couple "high importance" use cases:

1. View-Model to View-Model routing. We want the ability to trigger routing state changes from within the view models themselves so that integration (and testing) between view models is nice and easy.
2. Parameter passing. You should be able to pass whatever serializable parameter you want between view models.
3. Suspend/Resume. Your app has gotta be able to save its state and get right back to where it was.

## View-Model to View-Model routing

Supporting View-Model to View-Model routing is a tricky task. In particular, Android makes this situation hard because of its disconnected nature.

Because of the situation, I think it is best to enable View-Model to View-Model navigation by driving the interactions through the views themselves. 
We can provide common services for discovering, building, and passing parameters between View-Models, and let the view to view interaction be handled in the views themselves.

For example in Android (psueduo C#):

```csharp
class MyViewModel
{
    private IRouter router;
    // We inject the router into the view model.
    // In this scenario, each injected router is unique to each View-ViewModel pair.
    public MyViewModel(IRouter router)
    {
        this.router = router;
    }

    public void GoToOtherViewModel()
    {
        router.Navigate(typeof(MyOtherViewModel));
    }
}

public class MyActivity : Activity, IActivatable
{
    private MyViewModel viewModel;

    // The helper that this activity uses.
    // Helpers have two primary purposes:
    // 1. Create view models and provide parameters and state to them.
    // 2. Provide an interface for view models to call back to their host views to request navigation.
    private ViewModelRouterHelper router = new ViewModelRouterHelper();

    public MyActivity()
    {
        this.WhenActivated(d =>
        {
            // Register the router to be disposed when the view is deactivated.
            // This is used to trigger view model save state operations automatically.
            d(router);

            viewModel = (MyViewModel)router.CreateViewModel(typeof(MyViewModel), this);
            
            // Bind things...

            // Listen for navigation requests
            // The router helper automatically observes
            // events on RxApp.MainThreadScheduler for you.
            d(router.Navigate.FirstAsync(navArgs => 
            {
                var viewType = navArgs.ResolvedViewType;
                var viewModelType = navArgs.RequestedViewModelType;
                var parameters = navArgs.Parameters;
                var intent = new Intent(Context, viewType);
                // Put the parameters in the intent...
                StartActivity(intent);
            }));
        });
    }

    public override void OnCreate(Bundle savedInstanceState)
    {
        // other stuff

        // Grab the parameters that were passed from the previous view
        // and get the previous state that the view model saved.
        // Then tell the router (which is local to our view) to use them.
        // (WhenActivated gets called after this)
        var parameters = GetDataFromBundle(Intent.GetExtras());
        var state = GetDataFromBundle(savedInstanceState);
        router.SetParameters(typeof(MyViewModel), parameters);
        router.SetState(typeof(MyViewModel), state);

        // other stuff
    }

    public override void OnSaveInstanceState(Bundle savedInstanceState)
    {
        var viewModelState = router.GetViewModelState(viewModel);

        // put it into the bundle...

        base.OnSaveInstanceState(savedInstanceState);
    }

    private object GetDataFromBundle(Bundle parameters) 
    {
        // .. implementation ...
    }
}
```  

Or in iOS:

```csharp
class MyViewModel
{
    private IRouter router;
    public MyViewModel(IRouter router)
    {
        this.router = router;
    }

    public void GoToOtherViewModel()
    {
        router.Navigate(typeof(MyOtherViewModel));
    }
}

interface IRoutableViewController
{
    void SetParameters(object parameters);
}

public class MyViewController : UIViewController, IActivatable, IRoutableViewController
{
    private MyViewModel viewModel;
    private ViewModelRouterHelper router = new ViewModelRouterHelper();

    public MyViewController(IntPtr handle) : base(handle)
    {
        this.WhenActivated(d =>
        {
            d(router);
            viewModel = (MyViewModel)router.CreateViewModel(typeof(MyViewModel), this);

            // Bind things...

            // Listen for navigation requests
            // The router helper automatically observes
            // events on RxApp.MainThreadScheduler for you.
            d(router.Navigate.FirstAsync(navArgs => 
            {
                var viewType = navArgs.ResolvedViewType;
                var viewModelType = navArgs.RequestedViewModelType;
                var parameters = navArgs.Parameters;
                var nextController = CreateNewViewController(viewType);
                nextController.SetParameters(parameters);
                this.NavigationController.PushViewController(nextController, true);
            }));
        });
    }

    public void SetParameters(object parameters)
    {
        router.SetParameters(typeof(MyViewmodel), parameters);
    }
}
```