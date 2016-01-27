using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.Windsor;

namespace FeatureSwitcher.Windsor
{
    public class FeatureSwitcherFacility : IFacility
    {
        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            var handler = new FeatureSwitcherHandler();
            kernel.HandlerRegistered += handler.Kernel_HandlerRegistered;
            kernel.AddHandlerSelector(handler);
        }

        public void Terminate()
        {
        }
    }
}
