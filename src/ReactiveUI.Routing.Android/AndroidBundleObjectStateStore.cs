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
        public override Task<ObjectState> LoadStateAsync()
        {
            var latestBundle = GetLatestBundle();
            if (latestBundle == null) return Task.FromResult<ObjectState>(null);
            var json = latestBundle.GetString("__state");
            var state = JsonConvert.DeserializeObject<ObjectState>(json,
                Locator.Current.GetService<JsonSerializerSettings>());
            return Task.FromResult(state);
        }

        private static Bundle GetLatestBundle()
        {
            return Locator.Current.GetService<Bundle>();
        }

        protected override Task ClearStateAsync()
        {
            var latestBundle = GetLatestBundle();
            latestBundle?.Remove("__state");
            return Task.FromResult(0);
        }

        protected override Task SaveStateAsyncCore(ObjectState state)
        {
            var latestBundle = GetLatestBundle();
            if (latestBundle == null) throw new InvalidOperationException("Cannot save state in a null bundle.");
            var json = JsonConvert.SerializeObject(state, Locator.Current.GetService<JsonSerializerSettings>());
            latestBundle.PutString("__state", json);
            return Task.FromResult(0);
        }
    }
}