using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface that represents objects that can
    /// activate other objects.
    /// </summary>
    public interface IActivator
    {
        /// <summary>
        /// Activates a new object using the given activatable parameters.
        /// Returns the newly activated object.
        /// Throws an <see cref="ArgumentException"/> if the object could not be instantiated.
        /// </summary>
        /// <param name="parameters">The options that should be used to activate an object.</param>
        /// <returns>Returns the newly activated object.</returns>
        Task<object> ActivateAsync(TransitionParams parameters);
        /// <summary>
        /// Deactivates the given object.
        /// </summary>
        /// <param name="activated">The already activated object that should be deactivated.</param>
        /// <returns></returns>
        Task DeactivateAsync(object activated);
    }
}
