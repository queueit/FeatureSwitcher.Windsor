using System;

namespace FeatureSwitcher.Windsor
{
    internal class RegistrationMetadata
    {
        public Type Service { get; }
        public bool Enabled { get; }
        public IFeature Feature { get; }

        internal RegistrationMetadata(Type service, bool enabled, IFeature feature)
        {
            Service = service;
            this.Enabled = enabled;
            this.Feature = feature;
        }
    }
}