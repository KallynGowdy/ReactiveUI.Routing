using System;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a base class for <see cref="IActivatable{TParams}"/> objects.
    /// </summary>
    /// <typeparam name="TParams"></typeparam>
    public class ActivatableObject<TParams> : IActivatable<TParams>
        where TParams : new()
    {
        protected bool Initialized { get; private set; }

        protected virtual void InitCoreSync(TParams parameters)
        {
        }

        protected virtual Task InitCoreAsync(TParams parameters)
        {
            InitCoreSync(parameters);
            return Task.FromResult(0);
        }

        public async Task InitAsync(TParams parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            await InitCoreAsync(parameters);
            Initialized = true;
        }

        protected virtual void DestroyCoreSync()
        {
        }

        protected virtual Task DestroyCoreAsync()
        {
            DestroyCoreSync();
            return Task.FromResult(0);
        }

        public virtual async Task DestroyAsync()
        {
            await DestroyCoreAsync();
        }
    }
}