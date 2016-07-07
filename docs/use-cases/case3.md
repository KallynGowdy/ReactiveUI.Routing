# Use Case #3 - Master Detail

Suppose an application displays a list of email conversations. When a conversation is selected from this list, the full conversation contents are shown.
The behavior of this application differers based on the available screen space. Most notably, when the screen is "large enough", the selected conversation is shown adjacent
to the conversation list. (i.e. the "master-detail" interface pattern)

For most applications, they can simply use the Xamarin Forms Master-Detail Page, but we will still explore how this can be handled using this routing library.

The full expected behavior can be understood as a set of statements:

- The "master" page displays a list of selectable "detail" pages.
- When a "detail" page is selected from the "master" page, it is displayed.
- When a "detail" page is displayed, it follows one of two behaviors:
    - If there is enough horizontal space for both pages beside each other, then the "detail" page is displayed to the right of the "master" page.
    - Else, then the "detail" page is displayed in the full screen.
- By default, the first "detail" page is selected in the "master" page.
- When navigating backwards from a "detail" page the previous "detail" page becomes selected until only the first page remains.

This behavior therefore has different presentation requirements based on the available space, but maintains the same navigation requirements.

It could be represented with the following router builder and presenters:

```csharp
IRouterBuilder builder = ...;

builder
    // When showing the MasterViewModel, use the MasterPresenter and Navigate.
    .When<MasterViewModel>(route => route.Present<MasterPresenter>().Navigate())

    // When showing the ConversationViewModel, use the ConversationPresenter and Navigate.
    .When<ConversationViewModel>(route => route.Present<ConversationPresenter>().Navigate());

class MasterPresenter : Presenter<MasterViewModel>
{
    public override Task<IDisposable> PresentAsync(MasterViewModel viewModel, object hint)
    {
        // Determine whether or not to display the views together
        if(ShouldDisplayTogether())
        {
            return base.PresentViewAsync<MasterDetailView>(viewModel, hint);
        }
        else 
        {
            return base.PresentViewAsync<MasterView>(viewModel, hint);
        }
    }

    // Attempts to display the given view as the detail to this view.
    // Returns null if it could not inject the given view
    public async Task<IDisposable> InjectViewAsync(object innerView)
    {
        var masterDetailView = this.Presented as MasterDetailView;
        if(masterDetailView != null)
        {
            // PresentViewAsync is a helper function defined in the base
            // that activates the given inner view after "adding" it to the 
            // view tree. The given function handles adding the view to the view tree.
            // Returns a disposable that deactivates the inner view and recalls the function with null
            // to remove it from the view tree. 
            return await base.PresentViewIntoAsync(innerView, view => masterDetailView.Detail = view);
        }
        return null;
    }

    public bool ShouldDisplayTogether()
    {
        return ...;
    }
}

class ConversationPresenter : Presenter<ConversationViewModel>
{
    MasterPresenter parent;

    // The master presenter is obtained via dependency injection.
    public ConversationPresenter(MasterPresenter parent)
    {
        this.parent = parent;
    }

    public override async Task<IDisposable> PresentAsync(ConversationViewModel viewModel, object hint) 
    {
        // CreateViewAsync is another helper function defined in the base
        // that instantiates a view of the given type and assigns the given view model to it.
        // The hint is passed along as a potential differentiator to which view type is instantiated. 
        var view = await base.CreateViewAsync<ConversationView>(viewModel, hint);
        var result = await parent.InjectViewAsync(view);
        if(result != null)
        {
            return result;
        }
        else
        {
            // PresentViewAsync shows and activates the given view using the default behavior.
            // Other variants of this method can also create the specified view and assign the view model to it.
            return await base.PresentViewAsync(view);
        }
    }
}
```
