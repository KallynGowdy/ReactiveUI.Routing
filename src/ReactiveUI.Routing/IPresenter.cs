using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for objects that can create displays for given objects.
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// Presents the given view model using the given hint as an optional presenter-specific parameter.
        /// Returns a disposable that, when disposed, destroys the display created by the presenter.
        /// </summary>
        /// <param name="viewModel">The view model that should be displayed.</param>
        /// <param name="hint">The optional object that can communicate to the presenter how the view model should be displayed.</param>
        /// <returns></returns>
        Task<IDisposable> PresentAsync(object viewModel, object hint);
    }
}
