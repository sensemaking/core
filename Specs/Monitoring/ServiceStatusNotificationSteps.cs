using System.Serialization;
using System.Threading;
using NodaTime;
using NSubstitute;
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
            service_monitor = Substitute.For<IMonitorServices>();
            service_status = ServiceMonitor.Status.Empty;
            Logging.Configure(service_monitor.Info, logger);
        }

        private void a_service_monitor()
        {
            service_monitor.Availability().Returns(Availability.Up());
        }

        private void it_has_a_status()
        {
            service_status = Fake.AliveServiceMonitorStatus;
            service_monitor.GetStatus().Returns(service_status);
        }

        private void a_heartbeat_interval() { }

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
            new ServiceStatusNotifier(service_monitor, heartbeat);
            Thread.Sleep((int) heartbeat.Milliseconds - 50);
        }

        private void status_is_logged()
        {
            logger.Received().Information( new { Monitor = service_monitor.Info, LogEntry = service_status }.Serialize());
        }

        private void it_is_logged_again_after_heartbeat_interval()
        {
            logger.ClearReceivedCalls();
            Thread.Sleep((int) heartbeat.Milliseconds + 50);
            logger.Received().Information( new { Monitor = service_monitor.Info, LogEntry = service_status }.Serialize());
        }

        private void status_is_logged_as_information()
        {
            logger.Received().Information( new { Monitor = service_monitor.Info, LogEntry = service_status }.Serialize());
        }

        private void status_is_logged_as_a_warning()
        {
            logger.Received().Warning( new { Monitor = service_monitor.Info, LogEntry = service_status }.Serialize());
        }

        private void status_is_logged_as_an_error()
        {
            logger.Received().Error( new { Monitor = service_monitor.Info, LogEntry = service_status }.Serialize());
        }

        private void status_is_logged_as_fatal()
        {
            logger.Received().Fatal( new { Monitor = service_monitor.Info, LogEntry = service_status }.Serialize());
        }
    }
}