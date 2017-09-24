using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using ReactiveUI.Routing.Registrations;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Common extension methods for <see cref="IReactiveAppBuilder"/>.
    /// </summary>
    public static class ReactiveAppBuilderExtensions
    {
        /// <summary>
        /// The list of assembly qualified type names that should be loaded as dependencies automatically.
        /// </summary>
        public static readonly List<string> TypesToLoad = BuildTypesToLoadList();

        /// <summary>
        /// Adds dependencies needed to get ReactiveUI.Routing working on your platform.
        /// </summary>
        /// <param name="builder"></param>
        public static IReactiveAppBuilder AddReactiveRouting(this IReactiveAppBuilder builder)
        {
            foreach (var typeName in TypesToLoad)
            {
                builder.Add(new DynamicRegistration(typeName));
            }

            return builder;
        }

        /// <summary>
        /// Registers the given factory for the given type.
        /// </summary>
        /// <param name="builder">The app builder.</param>
        /// <param name="factory">The factory that should be registered.</param>
        /// <param name="registration">The type that the value should be registered under.</param>
        /// <param name="contract">The contract that should be used when requesting the object.</param>
        public static void Register(this IReactiveAppBuilder builder, Func<object> factory, Type registration, string contract = null)
        {
            builder.Add(new FactoryRegistration(factory, registration, contract));
        }

        /// <summary>
        /// Registers the given constant value for the given type.
        /// </summary>
        /// <param name="builder">The app builder.</param>
        /// <param name="constant">The constant that should be registered.</param>
        /// <param name="registration">The type that the value should be registered under.</param>
        /// <param name="contract">The contract that should be used when requesting the object.</param>
        public static void RegisterConstant(this IReactiveAppBuilder builder, object constant, Type registration, string contract = null)
        {
            builder.Add(new ConstantRegistration(constant, registration, contract));
        }

        internal static IReactiveAppBuilder AddReactiveUI(this IReactiveAppBuilder builder)
        {
            return builder.Add(new ReactiveUIDependencies());
        }

        internal static List<string> BuildTypesToLoadList()
        {
            var typeInfo = typeof(ReactiveAppBuilderExtensions).GetTypeInfo();
            var assemblyName = typeInfo.Assembly.GetName();
            var assemblies = new[]
            {
                "ReactiveUI.Routing.Core",
                "ReactiveUI.Routing.Android",
                "ReactiveUI.Routing.UWP",
                "ReactiveUI.Routing.WPF",
            };

            var fullAssemblyNames = assemblies.Select(a => assemblyName.FullName.Replace(assemblyName.Name, a));

            var typesToLoad = new[]
            {
                "ReactiveUI.Routing.CoreRoutingDependencies",
                "ReactiveUI.Routing.Android.AndroidRoutingDependencies",
                "ReactiveUI.Routing.UWP.UwpRoutingDependencies",
                "ReactiveUI.Routing.WPF.WpfRoutingDependencies",
            };

            return fullAssemblyNames.Zip(typesToLoad, (a, t) => new { assembly = a, type = t })
                .Select(x => $"{x.type}, {x.assembly}")
                .ToList();
        }
    }
}
