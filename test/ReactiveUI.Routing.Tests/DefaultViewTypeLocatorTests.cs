using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ReactiveUI.Routing.Tests
{
    public class DefaultViewTypeLocatorTests
    {
        public class BaseViewType<TViewModel> : IViewFor<TViewModel> where TViewModel : class
        {
            object IViewFor.ViewModel
            {
                get { return ViewModel; }
                set { ViewModel = (TViewModel) value; }
            }

            public TViewModel ViewModel { get; set; }
        }

        public class ViewModelType1
        {
        }

        public class ViewType1 : BaseViewType<ViewModelType1>
        {
        }

        public interface IViewModelType2
        {
        }

        public class ViewModelType2 : IViewModelType2
        {
        }

        public class ViewType2 : BaseViewType<IViewModelType2>
        {
        }

        public DefaultViewTypeLocatorTests()
        {
            Locator = new DefaultViewTypeLocator();
        }

        public DefaultViewTypeLocator Locator { get; }

        [Fact]
        public void Test_Finds_View_Type_That_Implements_IViewFor_ViewModelType()
        {
            var viewType = Locator.ResolveViewType(typeof(ViewModelType1));
            viewType.Should().Be<ViewType1>();
        }

        [Fact]
        public void Test_Finds_View_Type_That_Implements_IViewFor_IViewModelType()
        {
            var viewType = Locator.ResolveViewType(typeof(ViewModelType2));
            viewType.Should().Be<ViewType2>();
        }
    }
}
