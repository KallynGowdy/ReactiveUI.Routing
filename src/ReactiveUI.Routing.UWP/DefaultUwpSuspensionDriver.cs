using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Newtonsoft.Json;
using Splat;

namespace ReactiveUI.Routing.UWP
{
    /// <summary>
    /// Defines a <see cref="ISuspensionDriver"/> that stores suspension data
    /// in UWP <see cref="ApplicationData.LocalSettings"/>.
    /// </summary>
    public class DefaultUwpSuspensionDriver : ISuspensionDriver, IEnableLogger
    {
        /// <summary>
        /// The default settings key that the suspension driver uses.
        /// </summary>
        public const string DEFAULT_SETTINGS_KEY = "ReactiveUI.Routing.State";

        /// <summary>
        /// Gets or sets the type of data stored in the state.
        /// </summary>
        public Type StateType { get; set; } = typeof(ReactiveAppState);

        /// <summary>
        /// Gets or sets the key that should be used to store and load the settings.
        /// </summary>
        public string SettingsKey { get; set; } = DEFAULT_SETTINGS_KEY;

        private ApplicationDataContainer Settings => ApplicationData.Current.LocalSettings;

        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public IObservable<object> LoadState()
        {
            return Observable.Start(() =>
            {
                if (Settings.Values.ContainsKey(SettingsKey))
                {
                    var json = (string)Settings.Values[SettingsKey];
                    return JsonConvert.DeserializeObject(json, StateType, SerializerSettings);
                }

                return null;
            }).LoggedCatch(this, Observable.Defer(() => InvalidateState().Select(_ => (object)null)));
        }

        public IObservable<Unit> SaveState(object state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            return Observable.Start(() =>
            {
                Settings.Values[SettingsKey] = JsonConvert.SerializeObject(state, SerializerSettings);
                return Unit.Default;
            });
        }

        public IObservable<Unit> InvalidateState()
        {
            return Observable.Start(() =>
            {
                Settings.Values.Remove(SettingsKey);
                return Unit.Default;
            });
        }
    }
}
