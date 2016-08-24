using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Akavache;

namespace PresentationDemos.Services
{
    public class SettingsService : ISettingsService
    {
        public Task<int> GetMaxTodos()
        {
            return BlobCache.LocalMachine.Get("MAX_TODOS")
                .Select(b => BitConverter.ToInt32(b, 0))
                .Catch<int, KeyNotFoundException>(e => Observable.Return(50))
                .ToTask();
        }

        public Task SaveMaxTodos(int max)
        {
            return BlobCache.LocalMachine.Insert("MAX_TODOS", BitConverter.GetBytes(max)).ToTask();
        }
    }
}
