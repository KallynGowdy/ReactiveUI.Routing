namespace ReactiveUI.Routing.Tests
{
    public class TestViewModel : ActivatableObject<TestParams>
    {
        public new bool Initialized => base.Initialized;
        public bool Destroyed { get; private set; }
        protected override void DestroyCoreSync()
        {
            base.DestroyCoreSync();
            Destroyed = true;
        }
    }
}