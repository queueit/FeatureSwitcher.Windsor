using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Xunit;

namespace FeatureSwitcher.Windsor.Tests
{
    public class FeatureSwitcherRegistrationExtensionsTest
    {
        [Fact]
        public void FeatureSwitcherRegistrationExtensions_False_Test()
        {
            var actualService = this.Test(false);

            Assert.IsType<Service1>(actualService);
        }

        [Fact]
        public void FeatureSwitcherRegistrationExtensions_True_Test()
        {
            var actualService = this.Test(true);

            Assert.IsType<Service2>(actualService);
        }

        private IService Test(bool enabled)
        {
            var mockBehaviour = new MockBooleanBehaviour(enabled);
            FeatureSwitcher.Configuration.Features.Are.ConfiguredBy.Custom(mockBehaviour.Behaviour);

            WindsorContainer container = new WindsorContainer();

            container.Kernel.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IService>()
                    .WithServices(),
                Component
                    .For<IService>()
                    .AsFeatureSwitch<IService, TestFeature, Service1, Service2>());

            var actualService = container.Resolve<IService>();
            return actualService;
        }
    }
}
