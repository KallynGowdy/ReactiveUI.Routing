using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class RouterTests
    {
        private INavigator navigator;
        private IPresenter presenter;
        Router router;

        public RouterTests()
        {
            navigator = Substitute.For<INavigator>();
            presenter = Substitute.For<IPresenter>();
            router = new Router(navigator, presenter);
        }

        [Fact]
        public void Test_Ctor_Throws_Exception_When_No_Navigator_Is_Available()
        {
            Assert.Throws<InvalidOperationException>(() => new Router(null, Substitute.For<IPresenter>()));
        }

        [Fact]
        public void Test_Ctor_Throws_Exception_When_No_Presenter_Is_Available()
        {
            Assert.Throws<InvalidOperationException>(() => new Router(Substitute.For<INavigator>(), null));
        }

        [Fact]
        public async Task Test_ShowAsync_Throws_If_Router_Is_Not_Initialized()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await router.ShowAsync(typeof(TestViewModel), null);
            });
        }
    }
}
