using System;

namespace ReactiveUI.Routing.Presentation
{
    /// <summary>
    /// Extension methods for <see cref="IPresenterResolver"/> objects.
    /// </summary>
    public static class PresenterResolverExtensions
    {
        /// <summary>
        /// Registers a resolver to be used for presenter requests of the given type.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IDisposable Register<TRequest>(this IMutablePresenterResolver resolver,
            Func<TRequest, IPresenterFor<TRequest>> predicate)
            where TRequest : PresenterRequest
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return resolver.Register(request => HandleRequest(request, predicate));
        }

        public static IDisposable Register<TRequest>(this IMutablePresenterResolver resolver,
            IPresenterFor<TRequest> presenter)
            where TRequest : PresenterRequest
        {
            return resolver.Register<TRequest>(request => presenter);
        }

        private static IPresenterFor<T> HandleRequest<T>(PresenterRequest request, Func<T, IPresenterFor<T>> predicate)
            where T : PresenterRequest
        {
            T req = request as T;
            return req != null ? predicate(req) : null;
        }
    }
}
