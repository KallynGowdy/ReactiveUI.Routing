﻿using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a base class for <see cref="IActivatable{TParams}"/> objects.
    /// </summary>
    /// <typeparam name="TParams"></typeparam>
    public class ActivatableObject<TParams> : ReactiveObject, IActivatable<TParams>
        where TParams : new()
    {
        private readonly BehaviorSubject<TParams> onActivated;

        public virtual bool SaveInitParams => true;
        public bool Initialized => InitParams != null;
        public TParams InitParams { get; private set; }
        public IObservable<TParams> OnActivated => onActivated
            .Where(p => p != null);

        public ActivatableObject()
        {
            onActivated = new BehaviorSubject<TParams>(default(TParams));
        }

        protected virtual void InitCoreSync(TParams parameters)
        {
        }

        protected virtual Task InitCoreAsync(TParams parameters)
        {
            return Task.FromResult(0);
        }

        public async Task InitAsync(TParams parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            InitCoreSync(parameters);
            await InitCoreAsync(parameters);
            InitParams = parameters;
            onActivated.OnNext(parameters);
        }

        protected virtual void DestroyCoreSync()
        {
        }

        protected virtual Task DestroyCoreAsync()
        {
            return Task.FromResult(0);
        }

        object IActivatable.InitParams => InitParams;

        Task IActivatable.InitAsync(object parameters)
        {
            return InitAsync((TParams)parameters);
        }

        public async Task DestroyAsync()
        {
            DestroyCoreSync();
            await DestroyCoreAsync();
        }
    }
}