using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Newtonsoft.Json;
using Splat;

namespace ReactiveUI.Routing.UseCases.Android
{
    public class Store<T> : ISuspensionDriver, IEnableLogger
        where T : class
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        private const string Filename = "data.xml";
        private Context context;

        public Store(Context context)
        {
            this.context = context;
        }


        public IObservable<object> LoadState()
        {
            return Observable.Start<T>(() =>
            {
                var fileList = context.FileList();
                if (!fileList.Contains(Filename))
                {
                    return default(T);
                }
                try
                {
                    using (StreamReader s = new StreamReader(context.OpenFileInput(Filename)))
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
                using (StreamWriter s = new StreamWriter(context.OpenFileOutput(Filename, FileCreationMode.Private)))
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
                if (context.FileList().Contains(Filename))
                {
                    File.Delete(Filename);
                }
            }, RxApp.TaskpoolScheduler);
        }
    }
}
