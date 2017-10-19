using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing.UseCases.Common.ViewModels
{
    [DataContract]
    public class ContentViewModel
    {
        public string Text { get; set; } = "Hello, Presentation!";

        [IgnoreDataMember]
        public ReactiveCommand<Unit, Unit> ShowDetail { get; }

        [IgnoreDataMember]
        private IReactiveRouter router;

        public ContentViewModel(IReactiveRouter router = null)
        {
            this.router = router ?? Locator.Current.GetService<IReactiveRouter>();
            ShowDetail = ReactiveCommand.CreateFromTask(ShowDetailImpl);
        }

        public async Task ShowDetailImpl()
        {
            await router.Navigate(NavigationRequest.Forward(new DetailViewModel()));
        }
    }
}