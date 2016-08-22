using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a helper class that assists with implementing <see cref="ISuspensionNotifier"/> for specific platforms.
    /// </summary>
    public class SuspensionNotifierHelper : ISuspensionNotifier
    {
        public SuspensionNotifierHelper()
        {
            SaveStateSubject = new Subject<Unit>();
            SuspendSubject = new Subject<Unit>();
        }

        private Subject<Unit> SaveStateSubject { get; }
        private Subject<Unit> SuspendSubject { get; }
        public IObservable<Unit> OnSaveState => SaveStateSubject;
        public IObservable<Unit> OnSuspend => SuspendSubject;

        public void TriggerSaveState()
        {
            SaveStateSubject.OnNext(Unit.Default);
        }

        public void TriggerSuspension()
        {
            SuspendSubject.OnNext(Unit.Default);
        }
    }
}
