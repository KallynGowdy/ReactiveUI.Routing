using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing
{
    public class DefaultViewTypeLocator : IViewTypeLocator
    {
        private readonly Assembly[] searchableAssemblies;

        public DefaultViewTypeLocator() : this(null)
        {
        }

        public DefaultViewTypeLocator(params Assembly[] assemblies)
        {
            searchableAssemblies = assemblies ?? new Assembly[0];
        }

        public Type ResolveViewType(Type vmType)
        {
            return FindViewTypeFromAssembly(vmType, vmType.GetTypeInfo().Assembly) ??
                searchableAssemblies.Select(a => FindViewTypeFromAssembly(vmType, a)).FirstOrDefault(t => t != null);
        }

        private Type FindViewTypeFromAssembly(Type vmType, Assembly assembly)
        {
            return assembly.DefinedTypes.FirstOrDefault(t =>
                t.ImplementedInterfaces.Select(i => i.GetTypeInfo()).Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IViewFor<>) &&
                    MatchesViewModelType(i, vmType)))?.AsType();
        }

        private static bool MatchesViewModelType(TypeInfo interfaceType, Type viewModelType)
        {
            return interfaceType.GenericTypeArguments[0].GetTypeInfo().IsAssignableFrom(viewModelType.GetTypeInfo());
        }
    }
}
