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
    public class TestRoutedAppConfig : RoutedAppConfig
    {
        public override void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            base.RegisterDependencies(resolver);
            resolver.RegisterConstant(Substitute.For<IPhotosService>(), typeof(IPhotosService));
        }

        public override void CloseApp()
        {
            throw new NotImplementedException();
        }

        protected override ISuspensionNotifier BuildSuspensionNotifier()
        {
            return For<ISuspensionNotifier>();
        }

        protected override IObjectStateStore BuildObjectStateStore()
        {
            return For<IObjectStateStore>();
        }
    }
}
