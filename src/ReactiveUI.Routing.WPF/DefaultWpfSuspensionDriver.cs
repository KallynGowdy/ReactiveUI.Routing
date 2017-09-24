using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Splat;

namespace ReactiveUI.Routing.WPF
{
    /// <summary>
    /// Definesa <see cref="ISuspensionDriver"/> that stores suspension data in isolated storage.
    /// </summary>
    public class DefaultWpfSuspensionDriver : ISuspensionDriver, IEnableLogger
    {
        private string suspensionDataFileName = "ReactiveUI.Routing.State.json";

        /// <summary>
        /// Gets or sets the filename that should be used to store the data.
        /// </summary>
        public string SuspensionDataFileName
        {
            get { return suspensionDataFileName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Must not be null or whitespace. (Should be a valid filename)", nameof(value));
                suspensionDataFileName = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of data stored in the state.
        /// </summary>
        public Type StateType { get; set; } = typeof(ReactiveAppState);

        /// <summary>
        /// Gets or sets the <see cref="JsonSerializerSettings"/> that should be used when serializing and deserializing.
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        /// <summary>
        /// Gets or sets the <see cref="IsolatedStorageScope"/> that should be used.
        /// </summary>
        public IsolatedStorageScope Scope { get; set; } = IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly;

        public IObservable<object> LoadState()
        {
            return Observable.StartAsync(async () =>
            {
                using (var storage = Open())
                {
                    if (storage.FileExists(SuspensionDataFileName))
                    {
                        using (var stream = new StreamReader(storage.OpenFile(SuspensionDataFileName, FileMode.Open)))
                        {
                            var json = await stream.ReadToEndAsync();

                            return JsonConvert.DeserializeObject(json, StateType, SerializerSettings);
                        }
                    }

                    return null;
                }
            }).LoggedCatch(this, Observable.Defer(() => InvalidateState().Select(_ => (object)null)));
        }

        public IObservable<Unit> SaveState(object state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            return Observable.StartAsync(async () =>
            {
                using (var storage = Open())
                {
                    using (var stream =
                        new StreamWriter(storage.OpenFile(SuspensionDataFileName, FileMode.OpenOrCreate)))
                    {
                        var json = JsonConvert.SerializeObject(state, SerializerSettings);

                        await stream.WriteAsync(json);
                    }
                }
            });
        }

        public IObservable<Unit> InvalidateState()
        {
            return Observable.Start(() =>
            {
                using (var storage = Open())
                {
                    if (storage.FileExists(SuspensionDataFileName))
                    {
                        storage.DeleteFile(SuspensionDataFileName);
                    }
                }
            });
        }

        private IsolatedStorageFile Open()
        {
            return IsolatedStorageFile.GetStore(Scope, null, null);
        }
    }
}
