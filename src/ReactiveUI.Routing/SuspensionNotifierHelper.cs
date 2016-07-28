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
            SuspendSubject = new Subject<Unit>();
        }

        private Subject<Unit> SuspendSubject { get; }
        public IObservable<Unit> OnSuspend => SuspendSubject;

        public void TriggerSuspension()
        {
            SuspendSubject.OnNext(Unit.Default);
        }
    }
}
