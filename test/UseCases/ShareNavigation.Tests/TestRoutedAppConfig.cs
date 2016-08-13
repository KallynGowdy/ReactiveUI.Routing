using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using ReactiveUI.Routing;
using ShareNavigation.Services;
using Splat;
using static NSubstitute.Substitute;

namespace ShareNavigation.Tests
{
    public class TestRoutedAppConfig : CompositeRoutedAppConfig
    {
        public TestRoutedAppConfig() : base(new DefaultDependencies(), new ShareNavigationDependencies()) { }

        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            resolver.RegisterConstant(Substitute.For<IPhotosService>(), typeof(IPhotosService));
        }
    }
}
