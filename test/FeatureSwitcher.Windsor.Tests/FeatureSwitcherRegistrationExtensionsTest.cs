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

            Assert.IsType<ServiceDisabled>(actualService);
        }

        [Fact]
        public void FeatureSwitcherRegistrationExtensions_True_Test()
        {
            var actualService = this.Test(true);

            Assert.IsType<ServiceEnabled>(actualService);
        }

        [Fact]
        public void FeatureSwitcherRegistrationExtensions_Lifecycle_DisposeTransient_Test()
        {
            var mockBehaviour = new MockBooleanBehaviour(false);
            FeatureSwitcher.Configuration.Features.Are.ConfiguredBy.Custom(mockBehaviour.Behaviour);

            WindsorContainer container = new WindsorContainer();
            container.AddFacility<FeatureSwitcherFacility>();

            container.Kernel.Register(
                Component.For<ServiceConsumer>()
                    .LifestyleTransient(),
                FeatureSwitch.For<IService>()
                    .UsingFeature<TestFeature>()
                    .ImplementedBy<ServiceEnabled, ServiceDisabled>()
                    .Configure(c => c.LifestyleTransient()));

            var actualConsumer = container.Resolve<ServiceConsumer>();
            container.Release(actualConsumer);

            Assert.True(actualConsumer.Disposed);
            Assert.True(actualConsumer.Service.Disposed);
        }

        [Fact]
        public void FeatureSwitcherRegistrationExtensions_Lifecycle_DoNotDisposeSingleton_Test()
        {
            var mockBehaviour = new MockBooleanBehaviour(false);
            FeatureSwitcher.Configuration.Features.Are.ConfiguredBy.Custom(mockBehaviour.Behaviour);

            WindsorContainer container = new WindsorContainer();
            container.AddFacility<FeatureSwitcherFacility>();

            container.Kernel.Register(
                Component.For<ServiceConsumer>()
                    .LifestyleTransient(),
                FeatureSwitch.For<IService>()
                    .UsingFeature<TestFeature>()
                    .ImplementedBy<ServiceEnabled, ServiceDisabled>());

            var actualConsumer = container.Resolve<ServiceConsumer>();
            container.Release(actualConsumer);

            Assert.True(actualConsumer.Disposed);
            Assert.False(actualConsumer.Service.Disposed);
        }

        private IService Test(bool enabled)
        {
            var mockBehaviour = new MockBooleanBehaviour(enabled);
            FeatureSwitcher.Configuration.Features.Are.ConfiguredBy.Custom(mockBehaviour.Behaviour);

            WindsorContainer container = new WindsorContainer();
            container.AddFacility<FeatureSwitcherFacility>();

            container.Kernel.Register(
                FeatureSwitch.For<IService>()
                    .UsingFeature<TestFeature>()
                    .ImplementedBy<ServiceEnabled, ServiceDisabled>()
                    .Configure(c => c.LifestyleTransient()));

            var actualService = container.Resolve<IService>();
            return actualService;
        }
    }

    public class ServiceConsumer : IDisposable
    {
        public IService Service { get; }

        public ServiceConsumer(IService service)
        {
            Service = service;
        }

        public void Dispose()
        {
            this.Disposed = true;
        }

        public bool Disposed { get; set; }
    }
}
