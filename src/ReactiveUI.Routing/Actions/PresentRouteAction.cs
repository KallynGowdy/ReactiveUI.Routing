using System;
using System.Runtime.Serialization;

namespace ReactiveUI.Routing.Actions
{
    /// <summary>
    /// Defines a <see cref="IRouterAction"/> that instructs the router to present a view model.
    /// </summary>
    [DataContract]
    public class PresentRouteAction : IRouteAction
    {
        [DataMember]
        private string PresenterTypeName
        {
            get { return PresenterType?.AssemblyQualifiedName; }
            set
            {
                PresenterType = value != null ? Type.GetType(value) : null;
            }
        }
        [IgnoreDataMember]
        public Type PresenterType { get; set; }
        [DataMember]
        public object Hint { get; set; }
    }
}