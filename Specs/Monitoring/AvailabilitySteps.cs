using NSubstitute;
using Sensemaking.Bdd;
using Sensemaking.Monitoring;

namespace Sensemaking.Specs
{
    public partial class AvailabilitySpecs
    {
        private IMonitor instance_monitor;
        private Availability availability;

        protected override void before_each()
        {
            base.before_each();
            availability = null;
            instance_monitor = null;
        }

        private void available()
        {
            var monitor = Substitute.For<IMonitor>();
            monitor.Availability().Returns(Availability.Up());
            instance_monitor = monitor;
        }

        private void unavailable(MonitoringAlert alert)
        {
            var monitor = Substitute.For<IMonitor>();
            monitor.Availability().Returns(Availability.Down(alert));
            instance_monitor = monitor;
        }

        public void getting_availability()
        {
            availability = instance_monitor.Availability();
        }

        public void comparing_seperate_service_availability()
        {
            availability = instance_monitor.Availability();
        }

        private void it_is_fully_available()
        {
            availability.Status.should_be(Availability.State.Full);
        }

        private void it_is_unavailable()
        {
            availability.Status.should_be(Availability.State.None);
        }

        private void availability_is(bool available)
        {
            bool @true = availability;
            @true.should_be(available);
        }

        private void alerts(MonitoringAlert alert)
        {
            availability.Alerts.should_contain(alert);
        }
    }
}