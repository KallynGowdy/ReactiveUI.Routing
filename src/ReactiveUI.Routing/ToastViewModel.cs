using System.Reactive;
using System.Reactive.Linq;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a view model that represents a toast message.
    /// </summary>
    public class ToastViewModel : ReActivatableObject<ToastViewModel.Params, Unit>, IToastViewModel
    {
        private readonly ObservableAsPropertyHelper<string> message;

        public struct Params
        {
            public string Message { get; set; }
        }

        public string Message => message.Value;

        public ToastViewModel()
        {
            message = this.OnActivated.Select(p => p.Message)
                .ToProperty(this, vm => vm.Message);
        }
    }
}
