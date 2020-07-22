using System;
using System.Serialization;
using System.Threading;

namespace Sensemaking.Host.Monitoring
{
    public class ServiceStatusNotifier
    {
        private Timer Timer { get; set; }
        public IMonitorServices Monitor { get; }

        public ServiceStatusNotifier(IMonitorServices monitor)
        {
            Monitor = monitor;
            Timer = new Timer(e => Monitor.LogStatus(), null, TimeSpan.Zero, monitor.Heartbeat.ToDuration().ToTimeSpan());
        }
    }

    internal static class LogExtensions
    {
        internal static void LogStatus(this IMonitorServices monitor)
        {
            var status = monitor.GetStatus();
            switch (status.Health)
            {
                case ServiceMonitor.Status.Healthiness.Alive:
                    Logging.Information(status);
                    break;
                case ServiceMonitor.Status.Healthiness.Ill:
                    Logging.Warning(status);
                    break;
                case ServiceMonitor.Status.Healthiness.OnLastLegs:
                    Logging.Error(status);
                    break;
                case ServiceMonitor.Status.Healthiness.Dead:
                    Logging.Fatal(status);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}