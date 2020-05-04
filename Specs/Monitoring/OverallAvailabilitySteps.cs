using System.Collections.Generic;
using System.Linq;
using Sensemaking.Bdd;

namespace Sensemaking.Monitoring.Specs
{
    public partial class OverallAvailabilitySpecs
    {
        private IList<InstanceMonitor> instance_monitors_1;
        private IList<InstanceMonitor> instance_monitors_2;
        private Availability overall_availability;

        protected override void before_each()
        {
            base.before_each();
            overall_availability = null;
            instance_monitors_1 = new List<InstanceMonitor>();
            instance_monitors_2 = new List<InstanceMonitor>();
        }

        private void available_instance(IList<InstanceMonitor> monitors)
        {
            monitors.Add(new FakeInstanceMonitor());
        }

        private void unavailable_instance(ICollection<InstanceMonitor> monitors)
        {
            var instance = monitors.Count + 1;
            monitors.Add(new FakeInstanceMonitor($"{instance} is down."));
        }

        public void getting_overall_availability()
        {
            overall_availability = new MultiInstanceMonitor(instance_monitors_1).Availability() &
                                   new MultiInstanceMonitor(instance_monitors_2).Availability();
        }

        private void it_has_full_availability()
        {
            overall_availability.Status.should_be(Availability.State.Full);
        }

        private void it_has_no_availability()
        {
            overall_availability.Status.should_be(Availability.State.None);
        }

        private void it_has_reduced_availability()
        {
            overall_availability.Status.should_be(Availability.State.Reduced);
        }

        private void combined_alerts()
        {
            var all_alerts = new MultiInstanceMonitor(instance_monitors_1).Availability().Alerts
                .Concat(new MultiInstanceMonitor(instance_monitors_2).Availability().Alerts);
            overall_availability.Alerts.should_be(all_alerts);
        }
    }
}