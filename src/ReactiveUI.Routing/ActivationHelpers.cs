using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines helper functions for <see cref="IActivatable"/> objects.
    /// </summary>
    public static class ActivationHelpers
    {
        /// <summary>
        /// Suspends and destroys the given object if it is <see cref="IReActivatable"/> and returns a
        /// <see cref="ObjectState"/> object that represents a reactivatable version of the given object.
        /// </summary>
        /// <param name="obj">The object that should be suspended and destroyed.</param>
        /// <returns></returns>
        public static async Task<ObjectState> SuspendAndDestroyObjectAsync(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var state = await GetObjectStateAsync(obj);
            await DestroyObjectAsync(obj);
            return state;
        }

        /// <summary>
        /// Gets a <see cref="ObjectState"/> object that represents a reactivatable version of the given object.
        /// </summary>
        /// <param name="obj">The object that should be suspended and destroyed.</param>
        /// <returns></returns>
        public static async Task<ObjectState> GetObjectStateAsync(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var activationParams = GetActivationParams(obj);
            var state = await GetStateAsync(obj);
            return new ObjectState()
            {
                Params = activationParams,
                State = state
            };
        }

        /// <summary>
        /// Instantiates, initializes, and resumes the object described by the given <see cref="ObjectState"/>.
        /// </summary>
        /// <param name="savedState">The state that should be used to resurect the object.</param>
        /// <returns>Returns the resumed object.</returns>
        public static async Task<object> CreateInitAndResumeObjectAsync(ObjectState savedState)
        {
            if (savedState == null) throw new ArgumentNullException(nameof(savedState));
            var obj = CreateObject(savedState.Params);
            await InitAndResumeObjectAsync(obj, savedState);
            return obj;
        }

        /// <summary>
        /// Instantiates, initializes, and resumes the object described by the given <see cref="ObjectState"/>.
        /// </summary>
        /// <param name="savedState">The state that should be used to resurect the object.</param>
        /// <returns>Returns the resumed object.</returns>
        public static async Task<T> CreateInitAndResumeObjectAsync<T>(ObjectState savedState)
        {
            return (T)await CreateInitAndResumeObjectAsync(savedState);
        }

        /// <summary>
        /// Instantiates, initializes, and resumes the object described by the given <see cref="ObjectState"/>.
        /// </summary>
        /// <param name="parameters">The parameters that should be used to initialize the new object.</param>
        /// <param name="state">The state that should be used to resume the object.</param>
        /// <returns>Returns the resumed object.</returns>
        public static async Task<T> CreateInitAndResumeObjectAsync<T, TParams, TState>(TParams parameters,
            TState state)
            where T : IReActivatable<TParams, TState>
            where TParams : new()
            where TState : new()
        {
            return await CreateInitAndResumeObjectAsync<T>(new ObjectState()
            {
                Params = new ActivationParams()
                {
                    Type = typeof(T),
                    Params = parameters
                },
                State = state
            });
        }

        /// <summary>
        /// Initializes and resumes the object described by the given <see cref="ObjectState"/>.
        /// </summary>
        /// <param name="obj">The object that should be initialized and resumed.</param>
        /// <param name="savedState">The state that should be used to resume the object.</param>
        /// <returns></returns>
        public static async Task InitAndResumeObjectAsync(object obj, ObjectState savedState)
        {
            await InitObjectAsync(obj, savedState.Params.Params);
            await ResumeObjectStateAsync(obj, savedState.State);
        }

        /// <summary>
        /// Runs <see cref="IActivatable.DestroyAsync"/> on the object if it is an <see cref="IActivatable"/>.
        /// </summary>
        /// <param name="obj">The object that the destroy logic should be run for.</param>
        /// <returns></returns>
        public static async Task DestroyObjectAsync(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            await RunOnType<IActivatable>(obj, async activatable => await activatable.DestroyAsync());
        }

        /// <summary>
        /// Instantiates and initializes the object described by the given <see cref="ActivationParams"/>.
        /// </summary>
        /// <param name="parameters">The parameters that should be used to initialize the object.</param>
        /// <returns>Returns the created object.</returns>
        public static async Task<object> CreateAndInitObjectAsync(ActivationParams parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var obj = CreateObject(parameters);
            await InitObjectAsync(obj, parameters.Params);
            return obj;
        }

        /// <summary>
        /// Instantiates and initializes the object described by the given <see cref="ActivationParams"/>.
        /// </summary>
        /// <param name="parameters">The parameters that should be used to initialize the object.</param>
        /// <returns>Returns the created object.</returns>
        public static async Task<T> CreateAndInitObjectAsync<T, TParams>(TParams parameters)
            where T : IActivatable<TParams>
            where TParams : new()
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var activationParams = new ActivationParams()
            {
                Type = typeof(T),
                Params = parameters
            };
            return (T) await CreateAndInitObjectAsync(activationParams);
        }

        /// <summary>
        /// Initializes the given object if it is <see cref="IActivatable"/>.
        /// </summary>
        /// <param name="obj">The object that should be activated.</param>
        /// <param name="parameters">The parameters that should be passed to the object.</param>
        /// <returns></returns>
        public static async Task InitObjectAsync(object obj, object parameters)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (parameters == null) return;
            await RunOnType<IActivatable>(obj, async activatable => await activatable.InitAsync(parameters));
        }

        /// <summary>
        /// Initializes the given object if it is <see cref="IActivatable"/>.
        /// </summary>
        /// <param name="obj">The object that should be activated.</param>
        /// <param name="parameters">The parameters that should be passed to the object.</param>
        /// <returns></returns>
        public static Task InitObjectAsync<T, TParams>(T obj, TParams parameters)
            where T : IActivatable<TParams>
            where TParams : new()
        {
            return InitObjectAsync((object)obj, (object)parameters);
        }

        /// <summary>
        /// Retrieves the savable state from the given object if it is <see cref="IReActivatable"/>.
        /// </summary>
        /// <param name="obj">The object whose state should be retrieved.</param>
        /// <returns></returns>
        public static async Task<object> GetStateAsync(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return await RunOnType<IReActivatable, object>(obj, async activatable => await activatable.GetStateAsync());
        }

        /// <summary>
        /// Resumes the object with the given state if it is <see cref="IReActivatable"/>.
        /// </summary>
        /// <param name="obj">The object whose state should be resumed.</param>
        /// <param name="state">The state that should be given to the object.</param>
        /// <returns></returns>
        public static async Task ResumeObjectStateAsync(object obj, object state)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (state == null) return;
            await RunOnType<IReActivatable>(obj, async activatable => await activatable.ResumeAsync(state));
        }

        /// <summary>
        /// Retrieves a new instance of the type stored in the given <see cref="ActivationParams"/>.
        /// </summary>
        /// <param name="parameters">The parameters that describe the type that should be instantiated.</param>
        /// <returns>Returns the new object.</returns>
        public static object CreateObject(ActivationParams parameters)
        {
            var obj = Locator.Current.GetService(parameters.Type);
            if (obj == null) throw new InvalidOperationException($"Could not resolve an object of type: {parameters.Type} because Locator.Current.GetService({parameters.Type}) returned null. Make sure you have registered a factory for the type using Locator.CurrentMutable.Register(factory, {parameters.Type})");
            return obj;
        }

        /// <summary>
        /// Runs the given operation on the given object if the object is assignable to the given type.
        /// </summary>
        /// <typeparam name="T">The type that the object should be assignable to.</typeparam>
        /// <param name="obj">The object that the operation should be run on.</param>
        /// <param name="operation">The operation that should be run.</param>
        /// <returns></returns>
        public static async Task<TReturn> RunOnType<T, TReturn>(object obj, Func<T, Task<TReturn>> operation, TReturn @default = default(TReturn))
            where T : class
        {
            T activatable = obj as T;
            if (activatable != null)
            {
                return await operation(activatable);
            }
            return @default;
        }

        /// <summary>
        /// Runs the given operation on the given object if the object is assignable to the given type.
        /// </summary>
        /// <typeparam name="T">The type that the object should be assignable to.</typeparam>
        /// <param name="obj">The object that the operation should be run on.</param>
        /// <param name="operation">The operation that should be run.</param>
        /// <returns></returns>
        public static TReturn RunOnType<T, TReturn>(object obj, Func<T, TReturn> operation, TReturn @default = default(TReturn))
            where T : class
        {
            T activatable = obj as T;
            if (activatable != null)
            {
                return operation(activatable);
            }
            return @default;
        }

        /// <summary>
        /// Runs the given operation on the given object if the object is assignable to the given type.
        /// </summary>
        /// <typeparam name="T">The type that the object should be assignable to.</typeparam>
        /// <param name="obj">The object that the operation should be run on.</param>
        /// <param name="operation">The operation that should be run.</param>
        /// <returns></returns>
        public static async Task RunOnType<T>(object obj, Func<T, Task> operation)
            where T : class
        {
            T activatable = obj as T;
            if (activatable != null)
            {
                await operation(activatable);
            }
        }

        /// <summary>
        /// Runs the given operation on the given object if the object is assignable to the given type.
        /// </summary>
        /// <typeparam name="T">The type that the object should be assignable to.</typeparam>
        /// <param name="obj">The object that the operation should be run on.</param>
        /// <param name="operation">The operation that should be run.</param>
        /// <returns></returns>
        public static void RunOnType<T>(object obj, Action<T> operation)
            where T : class
        {
            T activatable = obj as T;
            if (activatable != null)
            {
                operation(activatable);
            }
        }

        private static ActivationParams GetActivationParams(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var parameters = RunOnType<IActivatable, object>(obj, activatable => activatable.SaveInitParams ? activatable.InitParams : null);
            return new ActivationParams()
            {
                Params = parameters,
                Type = obj.GetType()
            };
        }
    }
}



