using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureSwitcher.Windsor.Tests
{
    public class ServiceBase : IService
    {
        public bool Disposed { get; set; }

        public void Dispose()
        {
            this.Disposed = true;
        }
    }

    public class ServiceEnabled : ServiceBase
    {
    }

    public class ServiceDisabled : ServiceBase
    {
    }

    public interface IService : IDisposable
    {
        bool Disposed { get; }
    }
}
