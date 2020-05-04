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
            Timer = new Timer(e => Log.Information(Monitor.GetStatus().Serialize()), null, TimeSpan.Zero, monitor.Heartbeat.ToTimeSpan());
        }
    }
}