using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface that represents objects that can be suspended and resumed.
    /// </summary>
    public interface IReActivatable : IActivatable
    {
        /// <summary>
        /// Retrieves any state/data from the view model that needs to be preserved during app
        /// suspension. If null is returned, an exception is thrown.
        /// </summary>
        /// <returns></returns>
        Task<object> GetStateAsync();

        /// <summary>
        /// Resumes the view model state by restoring the stored state.
        /// </summary>
        /// <param name="storedState">The state that was previously stored.</param>
        /// <returns></returns>
        Task ResumeAsync(object storedState);
    }

    /// <summary>
    /// Defines a generic interface that represents objects that can be suspended and resumed.
    /// </summary>
    public interface IReActivatable<TParams, TState> : IActivatable<TParams>, IReActivatable
        where TParams : new()
        where TState : new()
    {
        /// <summary>
        /// Retrieves any state/data from the view model that needs to be preserved during app
        /// suspension. If null is returned, an exception is thrown.
        /// </summary>
        /// <returns></returns>
        new Task<TState> GetStateAsync();

        /// <summary>
        /// Resumes the view model state by restoring the stored state.
        /// </summary>
        /// <param name="storedState">The state that was previously stored.</param>
        /// <returns></returns>
        Task ResumeAsync(TState storedState);
    }
}
