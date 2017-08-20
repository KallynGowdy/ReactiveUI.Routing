using System;

namespace ReactiveUI.Routing.Presentation
{
    /// <summary>
    /// Defines an interface for objects that allow presenter resolvers to be dynamically registered and resolved.
    /// </summary>
    public interface IMutablePresenterResolver : IPresenterResolver
    {
        /// <summary>
        /// Registers a resolver to be used for presenter requests.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IDisposable Register(Func<PresenterRequest, IPresenter> predicate);
    }
}