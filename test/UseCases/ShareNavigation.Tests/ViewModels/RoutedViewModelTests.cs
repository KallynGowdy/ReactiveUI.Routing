using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReactiveUI.Routing;
using ShareNavigation.ViewModels;
using Xunit;

namespace ShareNavigation.Tests.ViewModels
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
