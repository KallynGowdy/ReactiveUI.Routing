namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for a presenter that can delegate work to other presenters.
    /// </summary>
    public interface IRootPresenter : IPresenter, IActivatable<RootPresenterParams>
    {
    }
}