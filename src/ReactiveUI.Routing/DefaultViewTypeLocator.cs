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
        public DefaultViewTypeLocator()
        {
        }

        public Type ResolveViewType(Type vmType)
        {
            return vmType.GetTypeInfo().Assembly.DefinedTypes.FirstOrDefault(t =>
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
