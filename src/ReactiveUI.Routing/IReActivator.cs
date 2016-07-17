using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface that represents objects that can suspend other objects.
    /// </summary>
    public interface IReActivator
    {
        /// <summary>
        /// Suspends the given object by retrieving the state that should be stored.
        /// Note that this method does not deactivate the given object.
        /// </summary>
        /// <param name="activated">The object that should be suspended.</param>
        /// <returns></returns>
        Task<object> SuspendAsync(object activated);
        /// <summary>
        /// Resumes an object of the type specified in the given parameters using the given state object.
        /// </summary>
        /// <param name="activated">The object that has been activated and should be resumed.</param>
        /// <param name="state">The state that should be used to resume the object.</param>
        /// <returns>Returns the object that was resumed.</returns>
        Task<object> ResumeAsync(object activated, object state);
    }
}