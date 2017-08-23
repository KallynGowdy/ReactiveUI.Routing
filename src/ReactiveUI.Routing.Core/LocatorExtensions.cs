using System;
using System.Linq;
using System.Reflection;
using ReactiveUI.Routing.Presentation;
using Splat;

namespace ReactiveUI.Routing
{
    public static class LocatorExtensions
    {
        public static void InitializeRouting(this IMutableDependencyResolver resolver)
        {
            new Registrations().Register(resolver);
        }

        // TODO: Make extensible through some interface or actual configuration setup
        //public static void InitializeRouting(this IMutableDependencyResolver resolver, params string[] assemblies)
        //{
        //    if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

        //    var a = assemblies.Union(new[]
        //    {
        //        "ReactiveUI.Routing.Core",
        //        "ReactiveUI.Routing.Android"
        //    }).Select(assembly => Assembly.Load(new AssemblyName(assembly)));
        //    return resolver.InitializeRouting(a);
        //}

        //public static void InitializeRouting(this IMutableDependencyResolver resolver, Assembly[] assemblies)
        //{
        //    if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
        //}
    }
}
