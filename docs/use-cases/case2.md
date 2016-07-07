### Use Case #2 - Open in Browser Navigation

Suppose an app displays contact information. The most prevalent screen in this app is the `ContactDetails` screen, which displays all of the related information for a contact. Among this information is a link to the contact's website. It is a requirement of the app that when the link is tapped/clicked, that the website opens in a web browser. 

Across the common moblie operating systems, the solution to this problem varies. In Android, you have to [use an `Intent`](http://stackoverflow.com/a/2201999/1832856). In iOS, you have to use [`UIApplication.openURL`](https://developer.apple.com/library/ios/documentation/UIKit/Reference/UIApplication_Class/#//apple_ref/occ/instm/UIApplication/openURL:). With UWP, you have to use [`LaunchUriAsync`](https://msdn.microsoft.com/library/windows/apps/windows.system.launcher.launchuriasync.aspx).

So what is the solution to this problem? Well, the first thing to note is that we are trying to show something. That means we should use a presenter. We can also note that we want the user to be able to get back to our application, so that involves navigation, which in turn involves app suspension.

We can conclude that our app needs three pieces of functionality:

1. A view model that represents the action of "show this URI".
2. It needs a presenter that calls the correct function on the correct platform.
3. It needs to be able to reopen the most recent screen when the app is resumed.

The first can be solved with a simple cross platform view model:

```csharp
class ShowInBrowserViewModel : PresentableViewModel<ShowInBrowserViewModel.Params>
{
	public class Params
	{
		public string Uri { get; set; }
	}
	
	public Uri Uri { get; set; }
	
	public override Task InitAsync(Params p)
	{
		this.Uri = new Uri(p.Uri);
		return Task.FromResult(0);
	}
}
```

The second can be solved with platform specific presenters for the above view model:

```csharp
// When showing the ShowInBrowserViewModel, use the available IShowInBrowserPresenter.
routerBuilder.When<ShowInBrowserViewModel>(route => route.Present<IShowInBrowserPresenter>());

// Android project
class AndroidShowInBrowserPresenter : IShowInBrowserPresenter
{
	private readonly Context context;
	
	public AndroidShowInBrowserPresenter(Context context)
	{
		this.context = context;
	}
	
	public Task<IDisposable> PresentAsync(ShowInBrowserViewModel viewModel, object hint)
	{
		var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(viewModel.Uri.ToString()));
		context.StartActivity(intent);
		return Task.FromResult<IDisposable>(null);
	}
}

// iOS Project
class IosShowInBrowserPresenter : IShowInBrowserPresenter
{
	public Task<IDisposable> PresentAsync(ShowInBrowserViewModel viewModel, object hint)
	{
		var application = UIApplication.SharedApplication;
		application.OpenUrl(new NSUrl(viewModel.Uri.ToString()));
		return Task.FromResult<IDisposable>(null);
	}
}

// UWP Project
class UwpShowInBrowserPresenter : IShowInBrowserPresenter
{
	public async Task<IDisposable> PresentAsync(ShowInBrowserViewModel viewModel, object hint)
	{
		await Launcher.LaunchUriAsync(viewModel.Uri).AsTask();
		return null;
	}
}
```

The third is already solved with the default suspend/resume pattern that is implemented in the default routers and navigation states.

Finally, the URL can be shown from the details page via some simple code:

```csharp
await router.ShowAsync<ShowInBrowserViewModel>(new ShowInBrowserViewModel.Params { Uri = url });
```
