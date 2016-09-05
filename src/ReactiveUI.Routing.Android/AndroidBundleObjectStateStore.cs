using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using ReactiveUI.Routing.Stores;
using Splat;

namespace ReactiveUI.Routing.Android
{
    public class AndroidBundleObjectStateStore : BaseObjectStateStore, IObjectStateStore
    {
        private readonly ObservableAsPropertyHelper<Bundle> latestBundle;
        private IObservable<Bundle> BundleObservable { get; }
        private Bundle LatestBundle => latestBundle.Value;

        public AndroidBundleObjectStateStore(IObservable<Bundle> bundleObservable = null)
        {
            BundleObservable = bundleObservable ?? Locator.Current.GetService<IObservable<Bundle>>();
            if (BundleObservable == null) throw new ArgumentNullException(nameof(bundleObservable));

            latestBundle = this.WhenAnyObservable(vm => vm.BundleObservable)
                .Where(b => b != null)
                .ToProperty(this, vm => vm.LatestBundle);
        }

        public override Task<ObjectState> LoadStateAsync()
        {
            if (LatestBundle == null) return Task.FromResult<ObjectState>(null);
            var json = LatestBundle.GetString("__state");
            var state = JsonConvert.DeserializeObject<ObjectState>(json,
                Locator.Current.GetService<JsonSerializerSettings>());
            return Task.FromResult(state);
        }

        protected override Task ClearStateAsync()
        {
            LatestBundle?.Remove("__state");
            return Task.FromResult(0);
        }

        protected override Task SaveStateAsyncCore(ObjectState state)
        {
            if (LatestBundle == null) throw new InvalidOperationException("Cannot save state in a null bundle.");
            var json = JsonConvert.SerializeObject(state, Locator.Current.GetService<JsonSerializerSettings>());
            LatestBundle.PutString("__state", json);
            return Task.FromResult(0);
        }
    }
}