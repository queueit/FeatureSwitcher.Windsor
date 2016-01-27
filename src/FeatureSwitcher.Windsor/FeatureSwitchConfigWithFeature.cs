using Castle.MicroKernel.Registration;

namespace FeatureSwitcher.Windsor
{
    public class FeatureSwitchConfigWithFeature<TService, TFeature>
        where TService : class
        where TFeature : IFeature, new()
    {
        public FeatureSwitcherDescriptor<TService, TFeature> ImplementedBy<TEnabled, TDisabled>()
            where TEnabled : TService
            where TDisabled : TService
        {
            return new FeatureSwitcherDescriptor<TService, TFeature>(
                new ComponentRegistration<TService>(typeof(TEnabled)),
                new ComponentRegistration<TService>(typeof(TDisabled)));
        }

    }
}