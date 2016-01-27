namespace FeatureSwitcher.Windsor
{
    public static class FeatureSwitch
    {
        public static FeatureSwitchConfig<TService> For<TService>()
            where TService : class
        {
            return new FeatureSwitchConfig<TService>();
        }
    }
}
