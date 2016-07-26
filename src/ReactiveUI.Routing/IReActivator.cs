using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface that represents objects that can suspend other objects.
    /// </summary>
    public interface IReActivator : IActivator
    {
        /// <summary>
        /// Suspends and deactivates the given object by retrieving the state that should be stored.
        /// Note that this method does not deactivate the given object.
        /// </summary>
        /// <param name="activated">The object that should be suspended.</param>
        /// <returns></returns>
        Task<ObjectState> SuspendAsync(object activated);

        /// <summary>
        /// Initializes and resumes an object of the type specified in the given parameters using the given state object.
        /// </summary>
        /// <param name="state">The state that was stored for the object that should be reactivated.</param>
        /// <returns>Returns the object that was resumed.</returns>
        Task<object> ResumeAsync(ObjectState state);
    }
}