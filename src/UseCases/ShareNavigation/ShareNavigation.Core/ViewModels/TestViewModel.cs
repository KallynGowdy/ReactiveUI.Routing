using System;
using System.Runtime.Serialization;
using ReactiveUI;

namespace ShareNavigation.ViewModels
{
    [DataContract]
    public class TestViewModel : ReactiveObject
    {
        string _TheGuid;
        [DataMember] public string TheGuid {
            get { return _TheGuid; }
            set { this.RaiseAndSetIfChanged(ref _TheGuid, value); }
        }

        public TestViewModel()
        {
            TheGuid = Guid.NewGuid().ToString();
        }
    }
}