using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for objects that are able to resolve view types from view model types.
    /// </summary>
    public interface IViewTypeLocator
    {
        Type ResolveViewType(Type viewModelType);
    }
}
