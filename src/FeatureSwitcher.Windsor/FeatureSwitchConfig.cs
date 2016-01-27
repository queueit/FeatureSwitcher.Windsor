namespace FeatureSwitcher.Windsor
{
    public class FeatureSwitchConfig<TService>
        where TService : class
    {
        public FeatureSwitchConfigWithFeature<TService, TFeature> UsingFeature<TFeature>()
            where TFeature : IFeature, new()
        {
            return new FeatureSwitchConfigWithFeature<TService, TFeature>();
        }
    }
}