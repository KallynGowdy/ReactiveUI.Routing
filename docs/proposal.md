# Proposal

Note that this proposal is a work in progress, subject to change. The end goal is to create a solution that solves all of the potential issues that can then be implemented.

## Overview

In these improvements, we take inspiration from MvvmCross for presenter-based view composition while providing our own solution to navigation that does not conflate the two concepts. The solutions to each of the goals are presented below:

- **G:** Cross platform logic. **S:** Utilize abstractions wherever possible, and limit platform specific logic to well defined extensibility points.
- **G:** Strong support for page/fragment/modal navigation. **S:** Use the notion of a presenter, which is able to obtain and manage views for view models.
- **G:** Async all the way. **S:** Use observables or tasks whenever a navigation or presentation opperation occurs.
- **G:** Support suspend/resume. **S:** Introduce a simple view model lifecycle that allows view models to store and load their current state.
- **G:** Minimize boilerplate code + Maximize readability. **S:** Encourage design that simplifies complex concepts and provide out of the box solutions for the most common usage patterns.

## What is navigation?

From Google:

**Navigation** is defined as:

1.  The process or activity of accurately ascertaining one's position and planning and following a route.
2.  The passage of ships.

So, when we talk about navigation, we are talking about "going" to something. This is important, because it helps us understand the behavior of navigation. 

*In particular, we know that when we navigate to something, we want to be able to navigate back.*

## What is presentation?

From Google:

**Presentation** is defined as:

1. The proffering or giving of something to someone, especially as part of a formal ceremony.
	- The manner or style in which something is given, offered, or displayed.
	- A formal introduction of someone, especially at court.
	- The action or right of formally proposing a candidate for a church benefice or other position.
	- A demonstration or display of a product or idea.

So, when we talk about presentation, we are talking about "showing" something. 

*When we present something, we want it to show up on screen.*

## A Dilemma

With the above definitions fresh in our minds, we can see that there are two different concepts at play. Navigation, which helps us get from place to place, and presentation, which helps us show things.

Often, when a developer writes:

```csharp
router.Navigate.ExecuteAsync(new MyViewModel());
```

they expect *both* things to happen.

As such, we have a predicament. We want to give the developer both what they expect and the option to choose. Therefore, we have two options:

1. Keep the existing API and work around it.
2. Introduce a new API that separates the two concepts.

I propose that we follow the 2nd option. We introduce a new API that resolves most, if not all, of the issues that exist with the current solution.

With this new API, we need a good model of what is happening. This model is split into three major parts:

1. `NavigationState`
2. `PresentationState`
3. `Router`

## Navigation State - [Full Article](./navigation-state.md)

The navigation state manages how view models are moved between and how the back button affects that movement. Quite simply, this object in in charge of two things:

1. Recording transitions between view models.
2. (Re)storing view model state.

The `IActivateViewModels` interface is a simple interface that signifies that an object can kick off the view model activation process.

## Presentation State - [Full Article](./presentation-state.md)

The presentation state manages which view models are being presented and how they are being presented.
It in turn also has three primary functions:

1. Discovering views for presented view models.
2. Creating and activating discovered views.
3. Disposing views.

The presentation state has no thing to do with storing view model state, only view state. 
In this respect, whenever the app is suspended, the router will need to kick off the correct view.

## Router - [Full Article](./router.md)
The router is in charge of managing the tight dance between `NavigationState` and `PresentationState`. In particular, it is able to resolve requests for `Show(vm)` and pipe them to either `NavigationState`, `PresentationState`, or both. In most cases, the router should obey custom rules which can be defined by a `RouterBuilder`.


## Platform Assumptions

In order to simplify the abstractions as much as possible, ReactiveUI.Routing will have to make several assumptions about the type of platforms that it will run on.
This relates mostly to how view models are constructed and how views are presented.

- ReactiveUI.Routing controls the navigation scheme. This means that navigation only occurs when ReactiveUI.Routing says it should.
- ReactiveUI.Routing controls the presentation scheme. This means that views are only shown when ReactiveUI.Routing says they should be.

These assumptions can become difficult to enforce for some platforms, most notably on Android where each Activity is treated like it's own Application. 
The solution is to provide bindings that replace/ignore the platform specific navigation/presentation code where it collides with ReactiveUI.Routing, and use the native ReactiveUI.Routing logic instead.
For example, replacing the native Android back button navigation to hook into ReactiveUI.