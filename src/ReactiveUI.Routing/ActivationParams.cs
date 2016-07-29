using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents parameters that are needed to activate an object.
    /// </summary>
    [DataContract]
    public sealed class ActivationParams
    {
        [DataMember]
        private string TypeName
        {
            get { return Type?.AssemblyQualifiedName; }
            set
            {
                Type = value != null ? System.Type.GetType(value) : null;
            }
        }

        /// <summary>
        /// Gets or sets the type of object that this transition represents.
        /// </summary>
        [IgnoreDataMember]
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the parameters that should be passed to the activated object.
        /// </summary>
        [DataMember]
        public object Params { get; set; }
    }
}