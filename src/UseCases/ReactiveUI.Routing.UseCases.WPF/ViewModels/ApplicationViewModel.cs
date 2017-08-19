using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Core.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    public class ApplicationViewModel : ReactiveObject
    {
        public IMutablePresenterResolver PresenterResolver { get; }

        public ApplicationViewModel()
        {
            PresenterResolver = new PresenterResolver();
        }

        public void Initialize()
        {
            Locator.CurrentMutable.RegisterConstant(this, typeof(ApplicationViewModel));
            Locator.CurrentMutable.RegisterConstant(PresenterResolver, typeof(IMutablePresenterResolver));
            Locator.CurrentMutable.RegisterConstant(PresenterResolver, typeof(IPresenterResolver));
        }
    }
}
