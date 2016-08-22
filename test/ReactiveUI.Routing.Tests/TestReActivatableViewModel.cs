namespace ReactiveUI.Routing.Tests
{
    public class TestReActivatableViewModel : ReActivatableObject<TestParams, TestState>
    {
        public TestState State { get; set; } = new TestState();

        protected override TestState SuspendCoreSync()
        {
            return State;
        }

        protected override void ResumeCoreSync(TestState storedState)
        {
            base.ResumeCoreSync(storedState);
            State = storedState;
        }
    }
}