namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents the hints that can be given to a <see cref="IToastPresenter"/>.
    /// </summary>
    public class ToastPresenterHints
    {
        public ToastPresenterDurationHints Duration { get; set; } = ToastPresenterDurationHints.Short;
    }

    /// <summary>
    /// Possible hints for <see cref="IToastPresenter"/> objects.
    /// </summary>
    public enum ToastPresenterDurationHints
    {
        Short,
        Long
    }
}