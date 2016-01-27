using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;

namespace FeatureSwitcher.Windsor
{
    public class FeatureSwitcherDescriptor<TService, TFeature> : IRegistration
        where TService : class
        where TFeature : IFeature, new()
    {
        private readonly ComponentRegistration<TService> _enabledRegistration;
        private readonly ComponentRegistration<TService> _disabledRegistration;

        internal FeatureSwitcherDescriptor(
            ComponentRegistration<TService> enabledRegistration,
            ComponentRegistration<TService> disabledRegistration)
        {
            var feature = Activator.CreateInstance<TFeature>();

            enabledRegistration.ExtendedProperties(new Property(FeatureSwitcherHandler.ExtendedPropertyName,
                new RegistrationMetadata(typeof(TService), true, feature)));
            disabledRegistration.ExtendedProperties(new Property(FeatureSwitcherHandler.ExtendedPropertyName,
                new RegistrationMetadata(typeof(TService), false, feature)));
            this._enabledRegistration = enabledRegistration;
            this._disabledRegistration = disabledRegistration;
        }

        public FeatureSwitcherDescriptor<TService, TFeature> Configure(Action<ComponentRegistration<TService>> configurationAction)
        {
            configurationAction.Invoke(this._enabledRegistration);
            configurationAction.Invoke(this._disabledRegistration);

            return this;
        }

        public FeatureSwitcherDescriptor<TService, TFeature> ConfigureEnabled(Action<ComponentRegistration<TService>> configurationAction)
        {
            configurationAction.Invoke(this._enabledRegistration);

            return this;
        }

        public FeatureSwitcherDescriptor<TService, TFeature> ConfigureDisabled(Action<ComponentRegistration<TService>> configurationAction)
        {
            configurationAction.Invoke(this._disabledRegistration);

            return this;
        }

        void IRegistration.Register(IKernelInternal kernel)
        {
            kernel.Register(
                this._enabledRegistration.Named(typeof(TService).FullName + "_FeatureSwitcherEnabled"), 
                this._disabledRegistration.Named(typeof(TService).FullName + "_FeatureSwitcherDisabled"));
        }
    }
}