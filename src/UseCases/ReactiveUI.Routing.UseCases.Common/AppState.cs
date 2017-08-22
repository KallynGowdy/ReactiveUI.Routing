using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Routing.Presentation;

namespace ReactiveUI.Routing.UseCases.WPF
{
    [DataContract]
    public class AppState
    {
        [DataMember]
        public AppPresentationState PresentationState { get; set; }

    }
}
