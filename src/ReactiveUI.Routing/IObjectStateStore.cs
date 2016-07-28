using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for an object that can save and store <see cref="ObjectState"/>.
    /// </summary>
    public interface IObjectStateStore
    {
        /// <summary>
        /// Saves the given state to the store.
        /// </summary>
        /// <param name="state">The state that should be saved.</param>
        /// <returns></returns>
        Task SaveState(ObjectState state);
        
        /// <summary>
        /// Loads the state from the store. Returns null if no state has been saved.
        /// </summary>
        /// <returns></returns>
        Task<ObjectState> LoadState();
    }
}
