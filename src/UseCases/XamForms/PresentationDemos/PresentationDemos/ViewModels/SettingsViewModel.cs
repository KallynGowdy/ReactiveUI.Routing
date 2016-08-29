using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationDemos.Services;
using ReactiveUI;
using ReactiveUI.Routing;
using Splat;

namespace PresentationDemos.ViewModels
{
    public class SettingsViewModel : ReActivatableObject<Unit, Unit>
    {
        public ISettingsService Settings { get; set; }
        private int maxTodos;

        public int MaxTodos
        {
            get { return maxTodos; }
            set { this.RaiseAndSetIfChanged(ref maxTodos, value); }
        }

        public ReactiveCommand<Unit> Save { get; }
        public ReactiveCommand<Unit> Load { get; }

        public SettingsViewModel(ISettingsService settings = null)
        {
            Settings = settings ?? Locator.Current.GetService<ISettingsService>();
            Load = ReactiveCommand.CreateAsyncTask(o => LoadImpl());
            var canSave = this.WhenAny(vm => vm.MaxTodos, max => max.Value > 0);
            Save = ReactiveCommand.CreateAsyncTask(canSave, o => SaveImpl());

            this.WhenAnyValue(vm => vm.MaxTodos)
                .Throttle(TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.Save);
        }

        private async Task LoadImpl()
        {
            MaxTodos = await Settings.GetMaxTodos();
        }

        private async Task SaveImpl()
        {
            await Settings.SaveMaxTodos(MaxTodos);
        }
    }
}
