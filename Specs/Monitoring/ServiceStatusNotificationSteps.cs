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
        private static ServiceMonitor.Status service_status;
        private static readonly Period heartbeat = Period.FromMilliseconds(200);
        private IMonitorServices service_monitor;
        private ILogger logger;

        protected override void before_all()
        {
            base.before_all();
            var spin_up_newtonsoft_cache = service_status.Serialize();
        }

        protected override void before_each()
        {
            base.before_each();
            logger = Substitute.For<ILogger>();
            Logging.Configure(logger);
            service_monitor = null;
            service_status = ServiceMonitor.Status.Empty;
        }

        private void a_service_monitor()
        {
            service_monitor = Substitute.For<IMonitorServices>();
            service_monitor.Heartbeat.Returns(Period.FromSeconds(20));
            service_monitor.Availability().Returns(Availability.Up());
        }

        private void it_has_a_status()
        {
            service_status = Fake.AliveServiceMonitorStatus;
            service_monitor.GetStatus().Returns(service_status);
        }

        private void a_heartbeat_interval()
        {
            service_monitor.Heartbeat.Returns(heartbeat);
        }

        private void its_status_is_alive()
        {
            service_status = Fake.AliveServiceMonitorStatus;
            service_monitor.GetStatus().Returns(service_status);
        }

        private void its_status_is_ill()
        {
            service_status = Fake.IllServiceMonitorStatus;
            service_monitor.GetStatus().Returns(service_status);
        }

        private void its_status_is_on_last_legs()
        {
            service_status = Fake.OnLastLegsServiceMonitorStatus;
            service_monitor.GetStatus().Returns(service_status);
        }

        private void its_status_is_dead()
        {
            service_status = Fake.DeadServiceMonitorStatus;
            service_monitor.GetStatus().Returns(service_status);
        }

        private void notifying_of_service_status()
        {
            new ServiceStatusNotifier(service_monitor);
            Thread.Sleep((int) heartbeat.Milliseconds - 50);
        }

        private void status_is_logged()
        {
            var e = Log.Logger.IsEnabled(LogEventLevel.Information);
            logger.Received().Information(service_status.Serialize());
        }

        private void it_is_logged_again_after_heartbeat_interval()
        {
            logger.ClearReceivedCalls();
            Thread.Sleep((int) heartbeat.Milliseconds + 50);
            logger.Received().Information(service_status.Serialize());
        }

        private void status_is_logged_as_information()
        {
            logger.Received().Information(service_status.Serialize());
        }

        private void status_is_logged_as_a_warning()
        {
            logger.Received().Warning(service_status.Serialize());
        }

        private void status_is_logged_as_an_error()
        {
            logger.Received().Error(service_status.Serialize());
        }

        private void status_is_logged_as_fatal()
        {
            logger.Received().Fatal(service_status.Serialize());
        }
    }
}