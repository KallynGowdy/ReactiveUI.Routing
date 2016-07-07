# Presentation State

The presentation state is in charge of displaying view models. 
It is in charge of selecting and creating the correct presenter for a view model which can then communicate with the platform to display the view model.

Really, the presentation state is a variation on a specialized presenter that manages other presenters. 

```csharp
interface IPresenter
{
	Task<IDisposable> PresentAsync(object viewModel, object hint);
}

interface IPresentationState : IPresenter
{
	// Registers the given view model type to be presented by the given presenter type.
	void Register(Type viewModelType, Type presenterType);
	// Unregisters the given view model type and coresponding presenter type. 
	void UnRegister(Type viewModelType, Type presenterType);
}
```

When it comes to creating views, that is all about the presenter. But a typical presenter will follow this sequence of steps:

1. Obtain a reference to the view host. This is the native component that actually does all of the displaying.
2. Determine the correct view for the view model.
3. Instantiate the view and assign the view model.
4. Add the view to the view tree via the view host.
5. Activate the view.
6. Return a disposable that undoes everything when disposed.
