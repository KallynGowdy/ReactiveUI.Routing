﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Core.Presentation;

namespace ReactiveUI.Routing.Core.Tests.Presentation
{
    public class TestPresenter : IPresenterFor<PresenterRequest>
    {
        public IObservable<PresenterResponse> Present(object request)
        {
            throw new NotImplementedException();
        }
    }
}
