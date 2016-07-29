using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using ReactiveUI.Routing;
using ReactiveUI.Routing.Stores;

namespace ShareNavigation
{
    public class AkavacheObjectStateStore : BaseObjectStateStore
    {
        private IBlobCache cache;
        private const string Key = "AppState";

        static AkavacheObjectStateStore()
        {
            BlobCache.ApplicationName = "ShareNavigation";
        }

        public AkavacheObjectStateStore(IBlobCache cache = null)
        {
            this.cache = cache ?? BlobCache.UserAccount;
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
            await cache.InvalidateObject<ObjectState>(Key);
        }

        protected override async Task SaveStateAsyncCore(ObjectState state)
        {
            await cache.InsertObject(Key, state);
        }
    }
}