
namespace Sensemaking.Monitoring
{
    public interface IMonitor
    {
        MonitorInfo Info { get; }
        Availability Availability();
    }


    public interface ICanBeMonitored
    {
        IMonitor Monitor { get; }
    }
}