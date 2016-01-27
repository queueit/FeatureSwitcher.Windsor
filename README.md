# FeatureSwitcher.Windsor
Castle Windsor IoC plugin for FeatureSwitcher https://github.com/mexx/FeatureSwitcher

#Usage

```c#
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

var actualService = container.Resolve<IService>();
```
