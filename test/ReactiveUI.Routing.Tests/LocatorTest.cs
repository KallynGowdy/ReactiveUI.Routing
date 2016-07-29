using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace ReactiveUI.Routing.Tests
{
    /// <summary>
    /// Defines a class for tests that use the Locator.
    /// </summary>
    public class LocatorTest : IDisposable
    {
        private readonly IMutableDependencyResolver originalResolver;
        protected IMutableDependencyResolver Resolver { get; }

        protected LocatorTest()
        {
            originalResolver = Locator.CurrentMutable;
            Locator.Current = Resolver = new ModernDependencyResolver();
            Resolver.InitializeSplat();
            Resolver.InitializeReactiveUI();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Resolver.Dispose();
            Locator.Current = originalResolver;
        }
    }
}
