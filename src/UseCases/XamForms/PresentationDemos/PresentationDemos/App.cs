using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI.Routing;
using ReactiveUI.Routing.XamForms;
using Xamarin.Forms;

namespace PresentationDemos
{
    public class App : RoutedApplication
    {
        private readonly IRoutedAppConfig platformDependencies;

        public App(IRoutedAppConfig platformDependencies = null)
        {
            this.platformDependencies = platformDependencies;
            StartHost();
        }

        protected override IRoutedAppConfig BuildAppConfig()
        {
            return new CompositeRoutedAppConfig(
                new DefaultDependencies(),
                platformDependencies,
                new DefaultXamFormsDependencies(this),
                new PresentationDemosDepedencies());
        }
    }
}
