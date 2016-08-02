using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using Newtonsoft.Json;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Stores;
using Splat;

namespace ShareNavigation
{
    public class AkavacheObjectStateStore : BaseObjectStateStore
    {
        private readonly IBlobCache cache;
        private const string Key = "AppState";

        static AkavacheObjectStateStore()
        {
            BlobCache.ApplicationName = "ShareNavigation";
        }

        public AkavacheObjectStateStore(IBlobCache cache = null)
        {
            this.cache = cache ?? BlobCache.LocalMachine;
        }

        public override async Task<ObjectState> LoadStateAsync()
        {
            try
            {
                return await cache.GetObject<ObjectState>(Key);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        protected override async Task ClearStateAsync()
        {
            await cache.InvalidateAll();
            await cache.Flush();
        }

        protected override async Task SaveStateAsyncCore(ObjectState state)
        {
            await cache.InsertObject(Key, state);
            await cache.Flush();
        }
    }
}