using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents the parameters that a router requires.
    /// </summary>
    [DataContract]
    public sealed class RouterParams
    {
        [DataMember]
        private Dictionary<string, RouteActions> ViewModelTypeNameMap
        {
            get { return ViewModelMap.ToDictionary(kv => kv.Key.AssemblyQualifiedName, kv => kv.Value); }
            set
            {
                ViewModelMap = value?.ToDictionary(kv => Type.GetType(kv.Key), kv => kv.Value);
            }
        }

        /// <summary>
        /// Gets or sets the map of view model types to the actions that should be taken on them.
        /// </summary>
        [IgnoreDataMember]
        public Dictionary<Type, RouteActions> ViewModelMap { get; set; }
        [DataMember]
        public object DefaultParameters { get; set; }

        [DataMember]
        private string DefaultViewModelTypeName
        {
            get { return DefaultViewModelType?.AssemblyQualifiedName; }
            set
            {
                DefaultViewModelType = value != null ? Type.GetType(value) : null;
            }
        }
        [IgnoreDataMember]
        public Type DefaultViewModelType { get; set; }
    }
}