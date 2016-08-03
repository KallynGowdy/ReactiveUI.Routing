using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Newtonsoft.Json;
using ReactiveUI.Routing.Stores;
using Splat;

namespace ReactiveUI.Routing.iOS
{
    public class NSCoderObjectStateStore : BaseObjectStateStore
    {
        public NSCoder Coder { get; set; }

        public override Task<ObjectState> LoadStateAsync()
        {
            if (Coder == null) return null;
            var str = (NSString) Coder.DecodeObject("__state");
            var state = JsonConvert.DeserializeObject<ObjectState>(str.ToString(),
                Locator.Current.GetService<JsonSerializerSettings>());
            return Task.FromResult(state);
        }

        protected override Task ClearStateAsync()
        {
            return Task.FromResult(0);
        }

        protected override Task SaveStateAsyncCore(ObjectState state)
        {
            if(Coder == null) throw new Exception("Cannot save state with a null coder.");
            var str = JsonConvert.SerializeObject(state, Locator.Current.GetService<JsonSerializerSettings>());
            Coder.Encode((NSString)str, "__state");
            return Task.FromResult(0);
        }
    }
}
