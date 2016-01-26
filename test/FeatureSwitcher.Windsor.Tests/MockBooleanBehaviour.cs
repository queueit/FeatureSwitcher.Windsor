namespace FeatureSwitcher.Windsor.Tests
{
    public class MockBooleanBehaviour
    {
        private readonly bool _enabled;

        public MockBooleanBehaviour(bool enabled)
        {
            _enabled = enabled;
        }

        public bool? Behaviour(Feature.Name name)
        {
            return this._enabled;
        }
    }
}