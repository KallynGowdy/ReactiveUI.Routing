using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ReactiveUI.Routing.Presentation;
using ReactiveUI.Routing.UseCases.Common;
using ReactiveUI.Routing.UseCases.Common.ViewModels;
using ReactiveUI.Routing.UWP;
using Splat;

namespace ReactiveUI.Routing.UseCases.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var a = new ReactiveAppBuilder()
                .AddReactiveRouting()
                .Add(new CommonUseCaseDependencies())
                .Add(new UwpViewDependencies())
                .ConfigureUwp(this, e)
                .Build();

            var content = Window.Current.Content as Frame;
            if (content == null)
            {
                content = new Frame();
                Window.Current.Content = content;
            }

            PagePresenter.RegisterHost(content);

            a.Presenter.PresentPage(new MainViewModel());

            Window.Current.Activate();

            a.Router.CanNavigate(NavigationRequest.Back())
                .Select(canNavigateBack => canNavigateBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed)
                .Do(visibility =>
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = visibility;
                })
                .Subscribe();
            
            SystemNavigationManager.GetForCurrentView().BackRequested += async (sender, args) =>
            {
                await a.Router.Navigate(NavigationRequest.Back());
            };
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
