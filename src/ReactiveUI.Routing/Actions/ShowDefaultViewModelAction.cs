using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing.Actions
{
    public class ShowDefaultViewModelAction : IRouterAction, IEquatable<ShowDefaultViewModelAction>
    {
        public bool Equals(ShowDefaultViewModelAction other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ShowDefaultViewModelAction) obj);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public static bool operator ==(ShowDefaultViewModelAction left, ShowDefaultViewModelAction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ShowDefaultViewModelAction left, ShowDefaultViewModelAction right)
        {
            return !Equals(left, right);
        }
    }
}
