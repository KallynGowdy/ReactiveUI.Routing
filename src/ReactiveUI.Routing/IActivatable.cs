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
    public interface IActivatable
    {
        /// <summary>
        /// Gets the parameters that were used to initialize object.
        /// </summary>
        object InitParams { get; }

        /// <summary>
        /// Gets whether the parameters used to initialize
        /// this object should be saved to the object state.
        /// </summary>
        bool SaveInitParams { get; }

        /// <summary>
        /// Gets whether the object is currently initialized.
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// Runs initialization logic for the view model.
        /// </summary>
        /// <param name="parameters">The parameters that are needed to initialize this view model. Should never be null.</param>
        /// <returns></returns>
        Task InitAsync(object parameters);

        /// <summary>
        /// Runs logic that is needed to cleanup this view model.
        /// </summary>
        /// <returns></returns>
        Task DestroyAsync();
    }

    /// <summary>
    /// Defines a generic interface that represents objects that can be activated.
    /// </summary>
    /// <typeparam name="TParams">The type of the parameter object that the object accepts.</typeparam>
    public interface IActivatable<TParams> : IActivatable
        where TParams : new()
    {
        /// <summary>
        /// Gets the parameters that were used to initialize object.
        /// </summary>
        new TParams InitParams { get; }

        /// <summary>
        /// Runs initialization logic for the view model.
        /// </summary>
        /// <param name="parameters">The parameters that are needed to initialize this view model. Should never be null.</param>
        /// <returns></returns>
        Task InitAsync(TParams parameters);
    }
}
