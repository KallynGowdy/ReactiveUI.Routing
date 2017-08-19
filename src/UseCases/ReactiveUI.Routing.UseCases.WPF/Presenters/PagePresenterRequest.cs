using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Core.Presentation;

namespace ReactiveUI.Routing.UseCases.WPF.Presenters
{
    public class PagePresenterRequest : PresenterRequest
    {
        public object ViewModel { get; set; }

        public PagePresenterRequest(object viewModel)
        {
            this.ViewModel = viewModel;
        }
    }
}
