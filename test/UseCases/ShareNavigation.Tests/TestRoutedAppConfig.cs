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
    public class TestRoutedAppConfig : IRoutedAppConfig
    {
        public void RegisterDependencies(IMutableDependencyResolver resolver)
        {
            resolver.RegisterConstant(Substitute.For<IPhotosService>(), typeof(IPhotosService));
            resolver.RegisterConstant(Substitute.For<IObjectStateStore>(), typeof(IObjectStateStore));
            resolver.RegisterConstant(Substitute.For<ISuspensionNotifier>(), typeof(ISuspensionNotifier));
        }

        public void CloseApp()
        {
            throw new NotImplementedException();
        }
    }
}
