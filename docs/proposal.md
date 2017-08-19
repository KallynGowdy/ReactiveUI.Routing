So now that I've had some time to think about this topic, I'm not 100% sure on a couple of the concepts that this library currently has:

## Issues
- The router is expected to be a single source of truth for managing the navigation stack. That is, you can call `ShowAsync()` and we're expecting more or less the same logic to run.
- The router has a simplistic model of navigation. It assumes that an application will only ever have a single navigation stack and that most navigation is primarily vertical. What about scenarios where you want to manage multiple navigation stacks (like a web browser)? How would you determine which navigation stack gets affected?
- The presenter framework is very simplistic. We're expecting presenters to effectively be factories for views. While that is definitely a useful abstraction to some extent, I think it might be a little too simplistic.  For example, it ignores the potentially complicated view state that presenters need to work around. Imagine the app startup phase. Presenters need to figure out how to get back to displaying the same content that was saved without simply replaying all of the actions.


## Potential Solutions

In reality, what we really want from a routing framework is to say "show this stuff to the user". When we're talking about stack-based navigation, we're really saying "show the user some of the stuff they were just viewing". From this perspective, we don't even need a navigation stack. Rather, we just need to track which information is currently being presented.

In the end, I imagine we come up with a framework like this:

- *Presenters* are in charge of constructing and managing views for view models. In this respect they act as decorators around the views, assisting with binding and lifecycle management. Because most applications don't control the construction of the first view, the root presenter is often also the root view.
- *ViewModels* are in charge of all of the core application logic. They are able to use existing patterns for binding data and events to views. In addition, they are in charge of requesting presentation changes from presenters.
- *Views* are in charge of displaying data to the user. In addition, they help bind events from the underlying system into the view models. For example, application lifecycle events are propagated from the system, into the views, and then into view models.

```csharp

// Presenters can be put into two categories
// 1. Imperative - these presenters build and bind specific views that you request.
// 2. Declarative - these presenters figure out which view(s) to bind upon request.
//
// In order to facilitate dynamic re-creation of presenters after suspension
// we take a request-response model. Presenters are retrieved via presenter
// request, which are serializeable. This allows us to save the current state of 
// presenters when the app is suspended. It also allows us to replay
// the presenters in order, thereby preserving the determinism.
//
// Note that the presenters themselves _do not_ store this state themselves.
// The presenters are just dumb boxes that do things for us. This state needs to be 
// stored somewhere else. For most apps, we expect that this state will be stored by the router.

// This is a type of imperative presenter that displays dialogs for the given view model.
interface IDialogPresenter : IPresenter
{
  IObservable<DialogResult> Present(DialogViewModel viewModel);
}

// This is a type of declarative presenter that figures out which child presenter to use
// for the view model you give it. 
// Declarative routers like this generally take advantage of imperative routers internally.
interface IPresenterResolver
{
  IPresenter Resolve(PresenterRequest viewModel);
}

// Suspend/resume strategy
// To handle app suspension, we need to provide an abstraction on top
// of the platform-specific life cycles.
//
// XamForms: Start->Sleep->Resume
// iOS:      Activated->ResignActivation->EnterBackground->Terminate
//           Activated->ResignActivation->EnterBackground->EnterForeground
// Android:  Create->Start->Resume->Pause->Stop->Destroy
//           Create->Start->Resume->Pause->Stop->Restart->Start
//
// UWP:      Activated->LeavingBackground->EnteredBackground->Suspending->Resume
//
// WPF:      N/A -> App life cycle is controlled by user.
//
// In particular, we're talking about handling app suspension transparently.
// Because most platforms force suspension on their respective apps, we need to provide
// a simple, easy, and sane way to handle this.
// Platforms such as WPF don't force suspension on their apps, but may need an easy way to save state.


class App : Application, IViewFor<ApplicationViewModel>
{
  
}

class Router : ReactiveObject
{
  
}
```
