using System;
using System.Threading;
using NodaTime;

namespace Sensemaking.Host.Monitoring
{
    public class ServiceStatusNotifier
    {
        private Timer Timer { get; set; }
        public IMonitorServices Monitor { get; }
        public Period Heartbeat { get; }

        public ServiceStatusNotifier(IMonitorServices monitor, Period heartbeat)
        {
            Monitor = monitor;
            Heartbeat = heartbeat;
            Timer = new Timer(e => Monitor.LogStatus(), null, TimeSpan.Zero, Heartbeat.ToDuration().ToTimeSpan());
        }
    }

    public static class LogExtensions
    {
        public static void LogStatus(this IMonitorServices monitor)
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