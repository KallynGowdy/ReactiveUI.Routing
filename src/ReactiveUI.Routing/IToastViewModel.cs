namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines an interface for view models that can present toast messages.
    /// </summary>
    public interface IToastViewModel
    {
        /// <summary>
        /// Gets the message for the toast.
        /// </summary>
        string Message { get; }
    }
}