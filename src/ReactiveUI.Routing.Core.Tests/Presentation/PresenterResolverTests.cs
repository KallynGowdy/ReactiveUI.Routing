using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReactiveUI.Routing.Core.Presentation;
using Xunit;

namespace ReactiveUI.Routing.Core.Tests.Presentation
{
    public class PresenterResolverTests
    {
        public PresenterResolver Subject { get; set; }

        public PresenterResolverTests()
        {
            Subject = new PresenterResolver();
        }

        [Fact]
        public void Test_Resolve_Does_Not_Allow_Nulls()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Subject.Resolve(null);
            });
        }

        [Fact]
        public void Test_Register_Does_Not_Allow_Nulls()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Subject.Register(null);
            });
        }

        [Fact]
        public void Test_Resolve_Returns_Null_When_Not_Found()
        {
            var presenter = Subject.Resolve(new PresenterRequest());

            presenter.Should().BeNull();
        }

        [Fact]
        public void Test_Register_Returns_Disposable()
        {
            var disposable = Subject.Register(request => null);

            disposable.Should().NotBeNull();
        }

        [Fact]
        public void Test_Register_Is_Called_To_Resolve_A_Presenter()
        {
            var presenter = new TestPresenter();
            Subject.Register(request => presenter);

            var resolved = Subject.Resolve(new PresenterRequest());

            resolved.Should().Be(presenter);
        }

        [Fact]
        public void Test_Disposable_From_Register_Causes_Function_To_No_Longer_Be_Called()
        {
            var disposable = Subject.Register(request => new TestPresenter());
            disposable.Dispose();

            var resolved = Subject.Resolve(new PresenterRequest());

            resolved.Should().BeNull();
        }

        [Fact]
        public void Test_Registered_Functions_Are_Called_In_Reverse_Order_They_Are_Registered()
        {
            var presenter = new TestPresenter();
            var other = new TestPresenter();
            Subject.Register(request => presenter);
            Subject.Register(request => other);

            var resolved = Subject.Resolve(new PresenterRequest());

            resolved.Should().Be(other);
        }

        [Fact]
        public void Test_Resolve_Short_Circuits_When_A_Presenter_Is_Returned()
        {
            var presenter = new TestPresenter();
            var other = new TestPresenter();

            var resolver1 = Substitute.For<Func<PresenterRequest, IPresenter>>();
            resolver1(Arg.Any<PresenterRequest>()).Returns(presenter);

            var resolver2 = Substitute.For<Func<PresenterRequest, IPresenter>>();
            resolver1(Arg.Any<PresenterRequest>()).Returns(other);

            Subject.Register(resolver1);
            Subject.Register(resolver2);

            Subject.Resolve(new PresenterRequest());

            resolver1.DidNotReceive()(Arg.Any<PresenterRequest>());
        }

        [Fact]
        public void Test_Register_With_Type_Param_Only_Gets_Called_When_Resolving_Requests_Assignable_To_Type()
        {
            Subject.Register<TestPresenterRequest>(request => new TestPresenter());

            var presenter = Subject.Resolve(new PresenterRequest());

            presenter.Should().BeNull();

            presenter = Subject.Resolve(new TestPresenterRequest());

            presenter.Should().NotBeNull();
        }
    }
}
