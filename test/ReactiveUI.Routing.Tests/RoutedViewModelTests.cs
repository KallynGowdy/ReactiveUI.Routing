using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public abstract class RoutedViewModelTests<TParams, TState> : LocatorTest
        where TState : new() where TParams : new()
    {
        public RoutedViewModelTests()
        {
            Router = Substitute.For<IRouter>();
            RoutedViewModel = new RoutedViewModel<TParams, TState>(Router);
        }
        public IRouter Router { get; }
        public RoutedViewModel<TParams, TState> RoutedViewModel { get; set; }

        [Fact]
        public void Test_Gets_Router_From_Locator()
        {
            Resolver.Register(() => Router, typeof(IRouter));
            var viewModel = new RoutedViewModel<TParams, TState>(null);

            viewModel.Router.Should().Be(Router);
        }
    }
}
