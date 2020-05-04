using Sensemaking.Monitoring;

namespace Sensemaking.Host.Monitoring
{
    public class ServiceDependency
    {
        public IMonitor Monitor { get; }

        public ServiceDependency(IMonitor monitor)
        {
            Monitor = monitor;
        }
    }
}