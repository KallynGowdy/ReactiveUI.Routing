using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF.ViewModels
{
    public class ApplicationViewModel : ReactiveObject
    {
        public ApplicationViewModel()
        {
        }

        public void Initialize()
        {
            Locator.CurrentMutable.RegisterRouting();
        }
    }
}
