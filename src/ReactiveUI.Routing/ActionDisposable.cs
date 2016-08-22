using System;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a simple implementation of <see cref="IDisposable"/> that executes the given action when <see cref="Dispose"/> is called.
    /// </summary>
    public class ActionDisposable : IDisposable
    {
        private bool isDisposed = false;
        private readonly object lockObj = new object();
        private readonly Action action;

        public ActionDisposable(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            this.action = action;
        }

        public void Dispose()
        {
            if (isDisposed) return;
            lock (lockObj)
            {
                if (isDisposed) return;
                action();
                isDisposed = true;
            }
        }
    }
}