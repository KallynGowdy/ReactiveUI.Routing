### Use Case #1 - Share Navigation

Suppose an app allows users to share photos. It has three primary pages: 

- The photo list
- The share page
- And the details page.

Additionally, there are five primary forms of navigation between the pages:

- The "Share" button on the photo list which takes the user to the share page.
- The "Upload" button on the share page which takes the user to the new photo's detail page.
- A photo on the photo list which takes the user to the photo's detail page.
- The back button on the share page which takes the user back to the list page.
- The back button on the detail page which also takes the user back to the list page.

In short, the navigation graph looks like this:

                     +--------------+
                     |              |
           +---------+     List     +---------+
           |         |              |         |
           |         +--------------+         |
           |                                  |
           |                                  |
           |                                  |
           |                                  |
    +------v------+                    +------v------+
    |             |         X          |             |
    |    Share    +---------X---------->    Detail   |
    |             |         X          |             |
    +-------------+                    +-------------+

This is what the router helps with. The route definitions for the above app would look something like this:

```csharp
IRouteBuilder builder = ...;

// When we show the ListViewModel,
// present the ListView and add the view model to the navigation stack.
builder.When<ListViewModel>(route => 
			route.Present<ListView>().Navigate())

// When we show the DetailViewModel,
// present the DetailView and add the view model to the navigation stack, removing
// all other view models from it until the ListViewModel is the parent.
	   .When<DetailViewModel>(route => 
			route.Present<DetailView>().NavigateFrom<ListViewModel>())

// When we show the ShareViewModel,
// present the ShareView and add the view model to the navigation stack.
	   .When<ShareViewModel>(route =>
		    route.Present<ShareView>().Navigate());

IRouter router = builder.Build();
// ...
await router.ShowAsync(typeof(ListViewModel), null);
```

Oh, look! We just implemented declarative routing! We've defined the conditions we want for our navigation and presentation scheme, and now they just work.