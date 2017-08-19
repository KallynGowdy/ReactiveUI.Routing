using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace ReactiveUI.Routing.Core.Presentation
{
    /// <summary>
    /// Defines an interface for presenters that can handle requests for the given type.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IPresenterFor<in TRequest> : IPresenter
        where TRequest : PresenterRequest
    {
    }
}
