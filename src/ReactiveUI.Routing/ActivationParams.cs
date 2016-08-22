using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents parameters that are needed to activate an object.
    /// </summary>
    [DataContract]
    public sealed class ActivationParams : IEquatable<ActivationParams>
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

        public bool Equals(ActivationParams other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && Equals(Params, other.Params);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ActivationParams && Equals((ActivationParams)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type?.GetHashCode() ?? 0) * 397) ^ (Params?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(ActivationParams left, ActivationParams right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ActivationParams left, ActivationParams right)
        {
            return !Equals(left, right);
        }
    }
}