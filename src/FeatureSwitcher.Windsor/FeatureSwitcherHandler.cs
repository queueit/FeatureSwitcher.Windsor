using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel;
using Castle.Windsor;

namespace FeatureSwitcher.Windsor
{
    public class FeatureSwitcherHandler : IHandlerSelector
    {
        internal const string ExtendedPropertyName = "FeatureSwitcher.Windsor";

        private readonly HashSet<Type> _featureSwitcherServiceTypes = new HashSet<Type>();

        internal void Kernel_HandlerRegistered(IHandler handler, ref bool stateChanged)
        {
            RegistrationMetadata metadata = handler
                .ComponentModel.ExtendedProperties[ExtendedPropertyName] as RegistrationMetadata;

            if (metadata == null)
                return;

            if (!this._featureSwitcherServiceTypes.Contains(metadata.Service))
                this._featureSwitcherServiceTypes.Add(metadata.Service);
        }

        public bool HasOpinionAbout(string key, Type service)
        {
            return this._featureSwitcherServiceTypes.Contains(service);
        }

        public IHandler SelectHandler(string key, Type service, IHandler[] handlers)
        {
            if (handlers.Length == 0)
                return null;

            var featureSwitcherHandlers = handlers
                .Where(handler => handler.ComponentModel.ExtendedProperties.Contains(ExtendedPropertyName))
                .Select(handler => new
                {
                    Handler = handler,
                    Metadata = handler.ComponentModel.ExtendedProperties[ExtendedPropertyName] as RegistrationMetadata
                })
                .ToArray();
                
            if (featureSwitcherHandlers.Length == 0)
                return null;

            if (featureSwitcherHandlers.Length != 2)
                throw new ComponentRegistrationException("Incorrect feature switch configuration");

            var feature = featureSwitcherHandlers[0].Metadata.Feature;

            return feature.Is().Enabled 
                ? featureSwitcherHandlers.FirstOrDefault(handler => handler.Metadata.Enabled)?.Handler 
                : featureSwitcherHandlers.FirstOrDefault(handler => !handler.Metadata.Enabled)?.Handler;
        }

    }
}
