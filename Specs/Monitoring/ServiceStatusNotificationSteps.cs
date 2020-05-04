using System;
using System.Diagnostics;
using System.Serialization;
using System.Threading;
using NodaTime;
using NSubstitute;
using Sensemaking.Bdd;
using Sensemaking.Host.Monitoring;
using Serilog;
using Serilog.Events;

namespace Sensemaking.Monitoring.Specs
{
    public partial class ServiceStatusNotificationSpecs
    {
        private static readonly ServiceMonitor.Status service_status = new ServiceMonitor.Status(new FakeMonitor().Info, Array.Empty<MonitorInfo>(),
            Availability.Down(AlertFactory.InstanceUnavailable(new MonitorInfo("AlertType", "AlertName", "AlertInstances"), "Weird shizzle happened")));

        private static readonly Duration heartbeat = Duration.FromMilliseconds(200);
        private IMonitorServices service_monitor;
        private ServiceMonitor.Status log_entry;

        protected override void before_all()
        {
            base.before_all();
            var spin_up_newtonsoft_cache = service_status.Serialize();
        }

        protected override void before_each()
        {
            base.before_each();
            Log.Logger = Substitute.For<ILogger>();
            service_monitor = null;
        }

        protected override void after_all()
        {
            Log.CloseAndFlush();
            base.after_all();
        }

        private void a_service_monitor()
        {
            service_monitor = Substitute.For<IMonitorServices>();
            service_monitor.Availability().Returns(Availability.Up());
            service_monitor.GetStatus().Returns(service_status);
        }

        private void a_heartbeat_interval()
        {
            service_monitor.Heartbeat.Returns(heartbeat);
        }

        private void notifying_of_service_status()
        {
            new ServiceStatusNotifier(service_monitor);
            Thread.Sleep(heartbeat.Milliseconds - 50);
        }

        private void status_is_logged()
        {
            Log.Logger.Received().Write(LogEventLevel.Information, service_status.Serialize());
        }

        private void it_is_logged_again_after_heartbeat_interval()
        {
            Log.Logger.ClearReceivedCalls();
            Thread.Sleep(heartbeat.Milliseconds + 50);
            Log.Logger.Received().Write(LogEventLevel.Information, service_status.Serialize());
        }
    }
}