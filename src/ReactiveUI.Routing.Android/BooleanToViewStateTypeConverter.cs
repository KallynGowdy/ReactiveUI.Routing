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
        public enum Hints
        {
            /// <summary>
            /// Defines that when the input value is false,
            /// that the output value should be <see cref="ViewStates.Gone"/>.
            /// </summary>
            PreferGone,
            /// <summary>
            /// Defines that when the input value is false,
            /// that the output value should be <see cref="ViewStates.Invisible"/>.
            /// </summary>
            PreferInvisible
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
            var falseState = hint.GetValueOrDefault() == Hints.PreferGone ? ViewStates.Gone : ViewStates.Invisible;
            try
            {
                var visible = (bool)@from;
                result = visible ? ViewStates.Visible : falseState;
                return true;
            }
            catch (InvalidCastException ex)
            {
                result = ViewStates.Visible;
                this.Log().WarnException($"Couldn't convert object to type: {toType}.", ex);
                return false;
            }
        }
    }
}