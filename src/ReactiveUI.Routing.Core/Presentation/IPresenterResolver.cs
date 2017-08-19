namespace ReactiveUI.Routing.Core.Presentation
{
    /// <summary>
    /// Defines an interface that represents objects that can resolve presenters for different requests.
    /// </summary>
    public interface IPresenterResolver
    {
        /// <summary>
        /// Resolves a presenter for the given request.
        /// </summary>
        /// <param name="presenterRequest">The presenter request.</param>
        /// <returns>The resolved presenter. Or null if none was found.</returns>
        IPresenter Resolve(PresenterRequest presenterRequest);
    }
}