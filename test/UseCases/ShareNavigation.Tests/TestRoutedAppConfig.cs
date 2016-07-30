using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing;
using static NSubstitute.Substitute;

namespace ShareNavigation.Tests
{
    public class TestRoutedAppConfig : RoutedAppConfig
    {
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
