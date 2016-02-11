# FeatureSwitcher.Windsor
Castle Windsor IoC plugin for FeatureSwitcher https://github.com/mexx/FeatureSwitcher. This plugin will toggle two concreate implementations of an interface (service) based on the state of the feature (Enabled/Disabled). The state of the feature is determined when the service is resolved.

#Usage
```c#
// Set up feature switcher - Enable feature 'TestFeature'
Features.Are
    .ConfiguredBy.Custom(Features.OfType<TestFeature>.Enabled)

// Create Windsor container
WindsorContainer container = new WindsorContainer();
// Register Facility
container.AddFacility<FeatureSwitcherFacility>();

// Register feature switcher service
container.Kernel.Register(
    FeatureSwitch
        .For<IService>()
        .UsingFeature<TestFeature>()
        .ImplementedBy<ServiceEnabled, ServiceDisabled>());

// Resolve service. An instance of ServiceEnabled will be returned 
// if the feature of type TestFeature is enabled. If disabled it 
// returns an instance of ServiceDisabled. (In this case an instance 
// of the ServiceEnabled class is returned as the feature is configured 
// to be enabled)
var actualService = container.Resolve<IService>();
```

#Windsor service configuration
```c#
// configure both implementations
container.Kernel.Register(
    FeatureSwitch
        .For<IService>()
        .UsingFeature<TestFeature>()
        .ImplementedBy<ServiceEnabled, ServiceDisabled>()
        .Configure(c => c.LifestyleTransient()));
```

```c#
// configure enabled implementations (ServiceEnabled)
container.Kernel.Register(
    FeatureSwitch
        .For<IService>()
        .UsingFeature<TestFeature>()
        .ImplementedBy<ServiceEnabled, ServiceDisabled>()
        .ConfigureEnabled(c => c.LifestyleTransient()));
```

```c#
// configure disabled implementations (ServiceDisabled)
container.Kernel.Register(
    FeatureSwitch
        .For<IService>()
        .UsingFeature<TestFeature>()
        .ImplementedBy<ServiceEnabled, ServiceDisabled>()
        .ConfigureDisabled(c => c.LifestyleTransient()));
```

#NuGet
Install-Package FeatureSwitcher.Windsor
