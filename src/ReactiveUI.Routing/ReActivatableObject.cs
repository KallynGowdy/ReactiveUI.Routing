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

        public async Task<TState> SuspendAsync()
        {
            var state = await SuspendCoreAsync();
            if (state == null) throw new InvalidReturnValueException($"{nameof(SuspendCoreAsync)} or {nameof(SuspendCoreSync)} must not return a null value.");
            return state;
        }

        Task IReActivatable.ResumeAsync(object storedState, IReActivator reActivator)
        {
            return ResumeAsync((TState) storedState, reActivator);
        }

        protected virtual void ResumeCoreSync(TState storedState)
        {
        }

        protected virtual Task ResumeCoreAsync(TState storedState, IReActivator reActivator)
        {
            ResumeCoreSync(storedState);
            return Task.FromResult(0);
        }

        public async Task ResumeAsync(TState storedState, IReActivator reActivator)
        {
            if (storedState == null) throw new ArgumentNullException(nameof(storedState));
            if (reActivator == null) throw new ArgumentNullException(nameof(reActivator));
            await ResumeCoreAsync(storedState, reActivator);
            resumed.OnNext(storedState);
        }

        async Task<object> IReActivatable.SuspendAsync()
        {
            return await SuspendAsync();
        }
    }
}
