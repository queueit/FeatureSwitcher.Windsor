using Castle.MicroKernel.Registration;

namespace FeatureSwitcher.Windsor
{
    public static class FeatureSwitcherRegistrationExtensions
    {
        public static ComponentRegistration<TInterface> AsFeatureSwitch<TInterface, TFeature, TDisabled, TEnabled>(
            this ComponentRegistration<TInterface> registration) 
            where TInterface : class
            where TFeature : IFeature, new()
            where TDisabled : class, TInterface
            where TEnabled : class, TInterface
        {
            return registration
                .UsingFactoryMethod<TInterface>((kernel) =>
                {
                    if (Feature<TFeature>.Is().Enabled)
                        return kernel.Resolve<TEnabled>();

                    return kernel.Resolve<TDisabled>();
                })
                .LifestyleTransient();
        }
    }
}
