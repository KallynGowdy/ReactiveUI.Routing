using System.Threading.Tasks;

namespace ReactiveUI.Routing.Stores
{
    /// <summary>
    /// Defines a base class for <see cref="IObjectStateStore"/> implementations.
    /// </summary>
    public abstract class BaseObjectStateStore : IObjectStateStore
    {
        public Task SaveStateAsync(ObjectState state)
        {
            return state == null ? ClearStateAsync() : SaveStateAsyncCore(state);
        }

        public abstract Task<ObjectState> LoadStateAsync();

        protected abstract Task ClearStateAsync();
        protected abstract Task SaveStateAsyncCore(ObjectState state);
    }
}
