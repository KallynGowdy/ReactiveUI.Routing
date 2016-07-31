using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a base class for <see cref="IReActivatable{TParams,TState}"/> objects.
    /// </summary>
    /// <typeparam name="TParams"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public class ReActivatableObject<TParams, TState> : ActivatableObject<TParams>, IReActivatable<TParams, TState>
        where TParams : new()
        where TState : new()
    {
        private readonly BehaviorSubject<TState> resumed;

        public ReActivatableObject()
        {
            this.resumed = new BehaviorSubject<TState>(default(TState));
        }

        protected IObservable<TState> Resumed => resumed.Where(s => s != null);

        protected virtual TState SuspendCoreSync()
        {
            return new TState();
        }

        protected virtual Task<TState> SuspendCoreAsync()
        {
            return Task.FromResult(SuspendCoreSync());
        }

        public async Task<TState> GetStateAsync()
        {
            var state = await SuspendCoreAsync();
            if (state == null) throw new InvalidReturnValueException($"{nameof(SuspendCoreAsync)} or {nameof(SuspendCoreSync)} must not return a null value.");
            return state;
        }

        Task IReActivatable.ResumeAsync(object storedState)
        {
            return ResumeAsync((TState) storedState);
        }

        protected virtual void ResumeCoreSync(TState storedState)
        {
        }

        protected virtual Task ResumeCoreAsync(TState storedState)
        {
            ResumeCoreSync(storedState);
            return Task.FromResult(0);
        }

        public async Task ResumeAsync(TState storedState)
        {
            if (storedState == null) throw new ArgumentNullException(nameof(storedState));
            await ResumeCoreAsync(storedState);
            resumed.OnNext(storedState);
        }

        async Task<object> IReActivatable.GetStateAsync()
        {
            return await GetStateAsync();
        }
    }
}
