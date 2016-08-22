using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Actions
{
    /// <summary>
    /// Defines an action that instructs the router to show a view model with parameters.
    /// </summary>
    public sealed class ShowViewModelAction : IRouterAction, IEquatable<ShowViewModelAction>
    {
        /// <summary>
        /// The parameters that should be used to activate the view model.
        /// </summary>
        public ActivationParams ActivationParams { get; set; }

        public bool Equals(ShowViewModelAction other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(ActivationParams, other.ActivationParams);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ShowViewModelAction && Equals((ShowViewModelAction) obj);
        }

        public override int GetHashCode()
        {
            return ActivationParams?.GetHashCode() ?? 0;
        }

        public static bool operator ==(ShowViewModelAction left, ShowViewModelAction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ShowViewModelAction left, ShowViewModelAction right)
        {
            return !Equals(left, right);
        }
    }
}
