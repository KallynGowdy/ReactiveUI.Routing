using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Splat;

namespace ReactiveUI.Routing.UseCases.WPF
{
    public class Store<T> : ISuspensionDriver, IEnableLogger
        where T : class
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        private const string Filename = "data.xml";

        public IObservable<object> LoadState()
        {
            return Observable.Start<T>(() =>
            {
                if (!File.Exists(Filename))
                {
                    return default(T);
                }
                try
                {
                    using (StreamReader s = new StreamReader(File.OpenRead(Filename)))
                    {
                        string json = s.ReadToEnd();
                        return JsonConvert.DeserializeObject<T>(json, settings);
                    }
                }
                catch (Exception e)
                {
                    this.Log().ErrorException("An error occured while loading the app state", e);
                    return default(T);
                }
            }, RxApp.TaskpoolScheduler);
        }

        public IObservable<Unit> SaveState(object state)
        {
            try
            {
                using (StreamWriter s = new StreamWriter(File.Open(Filename, FileMode.Create)))
                {
                    var json = JsonConvert.SerializeObject(state, settings);
                    s.Write(json);
                    s.Flush();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Observable.Return(Unit.Default);
        }

        public IObservable<Unit> InvalidateState()
        {
            return Observable.Start(() =>
            {
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }
            }, RxApp.TaskpoolScheduler);
        }
    }
}
