using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Actions
{
    /// <summary>
    /// Defines a <see cref="IRouterAction"/> that specifies that the router should navigate backwards.
    /// </summary>
    public class NavigateBackAction : IRouterAction, IEquatable<NavigateBackAction>
    {
        public bool Equals(NavigateBackAction other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NavigateBackAction) obj);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public static bool operator ==(NavigateBackAction left, NavigateBackAction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NavigateBackAction left, NavigateBackAction right)
        {
            return !Equals(left, right);
        }
    }
}
