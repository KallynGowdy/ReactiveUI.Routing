using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface that represents objects that can be activated.
    /// </summary>
    /// <typeparam name="TParams">The type of the parameter object that the object accepts.</typeparam>
    public interface IActivatable<in TParams>
        where TParams : new()
    {
        /// <summary>
        /// Runs initialization logic for the view model.
        /// </summary>
        /// <param name="parameters">The parameters that are needed to initialize this view model. Should never be null.</param>
        /// <returns></returns>
        Task InitAsync(TParams parameters);

        /// <summary>
        /// Runs logic that is needed to cleanup this view model.
        /// </summary>
        /// <returns></returns>
        Task DestroyAsync();
    }
}
