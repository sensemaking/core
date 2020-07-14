using System;
using System.Diagnostics;
using System.Serialization;
using System.Threading;
using Serilog;

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

    internal static class Logging
    {
        internal static void LogStatus(this IMonitorServices monitor)
        {
            var status = monitor.GetStatus();
            switch (status.Health)
            {
                case ServiceMonitor.Status.Healthiness.Alive:
                    Log.Information(status.Serialize());
                    break;
                case ServiceMonitor.Status.Healthiness.Ill:
                    Log.Warning(status.Serialize());
                    break;
                case ServiceMonitor.Status.Healthiness.OnLastLegs:
                    Log.Error(status.Serialize());
                    break;
                case ServiceMonitor.Status.Healthiness.Dead:
                    Log.Fatal(status.Serialize());
                    break;
            }
        }
    }
}