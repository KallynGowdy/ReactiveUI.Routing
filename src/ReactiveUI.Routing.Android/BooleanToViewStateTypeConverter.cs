using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Splat;

namespace ReactiveUI.Routing.Android
{
    /// <summary>
    /// Defines a <see cref="IBindingTypeConverter"/> between <see cref="Boolean"/> values 
    /// and <see cref="ViewStates"/> values.
    /// </summary>
    public class BooleanToViewStateTypeConverter : IBindingTypeConverter
    {
        [Flags]
        public enum Hints
        {
            /// <summary>
            /// Defines that when the input value is false,
            /// that the output value should be <see cref="ViewStates.Gone"/>.
            /// </summary>
            GoneWhenFalse = 1 << 0,

            /// <summary>
            /// Defines that when the input value is false,
            /// that the output value should be <see cref="ViewStates.Invisible"/>.
            /// </summary>
            InvisibleWhenFalse = 1 << 1,

            /// <summary>
            /// Defines that when the input value is false,
            /// that the output value should be <see cref="ViewStates.Visible"/>.
            /// </summary>
            VisibleWhenFalse = 1 << 2,

            /// <summary>
            /// Defines that when the input value is true,
            /// that the output value should be <see cref="ViewStates.Gone"/>.
            /// </summary>
            GoneWhenTrue = 1 << 3,

            /// <summary>
            /// Defines that when the input value is true,
            /// that the output value should be <see cref="ViewStates.Invisible"/>.
            /// </summary>
            InvisibleWhenTrue = 1 << 4,

            /// <summary>
            /// Defines that when the input value is true,
            /// that the output value should be <see cref="ViewStates.Visible"/>.
            /// </summary>
            VisibleWhenTrue = 1 << 5,

            /// <summary>
            /// Defines that the control should be <see cref="ViewStates.Visible"/> when the value is true,
            /// and <see cref="ViewStates.Gone"/> when the value is false.
            /// </summary>
            Default = VisibleWhenTrue | GoneWhenFalse,

            /// <summary>
            /// Defines that the control should be <see cref="ViewStates.Gone"/> when the value is true,
            /// and <see cref="ViewStates.Visible"/> when the value is false.
            /// </summary>
            Inverted = VisibleWhenFalse | GoneWhenTrue
        }

        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            if (fromType == typeof(bool) && toType == typeof(ViewStates))
            {
                return 100;
            }
            return 0;
        }

        public bool TryConvert(object @from, Type toType, object conversionHint, out object result)
        {
            var hint = conversionHint as Hints?;
            var trueState = GetTrueState(hint);
            var falseState = GetFalseState(hint);
            try
            {
                var visible = (bool)@from;
                result = visible ? trueState : falseState;
                return true;
            }
            catch (InvalidCastException ex)
            {
                result = ViewStates.Visible;
                this.Log().WarnException($"Couldn't convert object to type: {toType}.", ex);
                return false;
            }
        }

        private ViewStates GetTrueState(Hints? conversionHint)
        {
            if (conversionHint.HasValue)
            {
                var hint = conversionHint.Value;
                if (hint.HasFlag(Hints.GoneWhenTrue))
                {
                    return ViewStates.Gone;
                }
                else if (hint.HasFlag(Hints.InvisibleWhenTrue))
                {
                    return ViewStates.Invisible;
                }
                else if (hint.HasFlag(Hints.VisibleWhenTrue))
                {
                    return ViewStates.Visible;
                }
            }
            return ViewStates.Visible;
        }

        private ViewStates GetFalseState(Hints? conversionHint)
        {
            if (conversionHint.HasValue)
            {
                var hint = conversionHint.Value;
                if (hint.HasFlag(Hints.GoneWhenFalse))
                {
                    return ViewStates.Gone;
                }
                else if (hint.HasFlag(Hints.InvisibleWhenFalse))
                {
                    return ViewStates.Invisible;
                }
                else if (hint.HasFlag(Hints.VisibleWhenFalse))
                {
                    return ViewStates.Visible;
                }
            }
            return ViewStates.Gone;
        }
    }
}