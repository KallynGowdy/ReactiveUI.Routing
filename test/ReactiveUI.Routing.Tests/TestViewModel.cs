namespace ReactiveUI.Routing.Tests
{
    public class TestViewModel : ActivatableObject<TestParams>
    {
        public new bool Initialized => base.Initialized;
    }
}