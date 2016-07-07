# Navigation State

As mentioned in the propsal document, the `NavigationState` is in charge of managing activatable view models.

## ViewModel Activation

An activatable view model is, quite simply, a view model that implements the `IActivatable` interface defined below:

```csharp
interface IActivatable<TParams>
{
    Task InitAsync(TParams parameters);
    Task DestroyAsync();
}
```

As can be inferred from the above interface, view model activation takes place in two total steps:

1. Initialize the view model. Quite simply, calling the parameterless constructor on the view model.
2. Call `InitAsync` with the specified parameters.

Boom! Done. Your view model is now ready for use. Or in actuality, is already being used.

Now, for the suspend/resume strategy, we need to add another interface to support custom storage logic.

```csharp
interface IReActivatable<TParams, TState> : IActivatable<TParams>
{
    Task<TState> SuspendAsync();
    Task ResumeAsync(TState storedState);
}
```

View models which support reactivation, make a slight change to the above activation list. When a previously suspended view model is being resumed,
`ResumeAsync` is called immediately after initialization. Meaning that once `InitAsync` is called, the view model has been started.

On deactivation, only one method needs to be called: `DestroyAsync`.
When a view model is being deactivated, it will never be resurected, which means that it doesn't need to save state.
If a view model does want to do things, it can perform them in `DestroyAsync`.

On suspension, two methods are called:

1. (Optional) If the view model implements `IReActivatable`, then `SuspendAsync()` is called and the returned state is stored.
2. The deactivation sequence is run via calling `DestroyAsync`.

Enough of view model activation. Let's talk about the `NavigationState` model, whose interface is defined below:

```csharp
sealed class ActivatableParams 
{
	public Type Type { get; }
	public object Params { get; }
	    
    public ViewTransition(
	    Type type, 
	    object vmParams)
    {
        Type = type;
		Params = vmParams;
    }
}

// Helper interfaces that the navigation state uses to activate and deactivate view models.
interface IActivator
{
    Task<object> ActivateAsync(ActivatableParams parameters);
}

interface IResumer : IActivator
{
    Task<object> ResumeAsync(ActivatableParams parameters, object state);
}

interface IDeactivator
{
    Task DeactivateAsync(object activated);
}

interface ISuspender : IDeactivator
{
    Task<object> SuspendAsync(object activated);
}

interface INavigationState : IReActivatable<Unit, ActivatableParams[]>
{
	IReadOnlyCollection<ActivatableParams> TransitionStack { get; }
	Task PushAsync(ActivatableParams transition);
	Task PopAsync();
	Task GroupAsync(Func<Task> grouping);
	IObservable<ActivatableParams> Transitioned { get; }
}
```