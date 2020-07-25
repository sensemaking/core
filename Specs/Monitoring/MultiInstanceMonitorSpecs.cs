using System.Linq;
using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Monitoring.Specs
{
    public partial class MultiInstanceMonitorSpecs : Specification
    {
        private static readonly Alert unavailable_alert =
            AlertFactory.ServiceUnavailable(Fake.AnInstanceMonitor.Info, "It's down Bob");

        private static readonly Alert unavailable_alert_2 =
            AlertFactory.ServiceUnavailable(Fake.AnInstanceMonitor.Info, "It's down Bill");

        [Test]
        public void uses_monitored_monitor_name_and_type_as_monitoring_info()
        {
            Given(available_instance);
            Then(monitor_info_has_name_and_type);
        }

        [Test]
        public void provides_list_of_all_instances_monitored()
        {
            Given(available_instance);
            Then(lists_all_monitored_instances);
        }

        [Test]
        public void only_monitors_across_instance_monitors()
        {
            Given(a_non_instance_monitor);
            Then(multi_instance_monitor_cannot_be_used);
        }

        [Test]
        public void a_set_of_available_instances_are_fully_available()
        {
            Given(available_instance);
            And(available_instance);
            When(getting_availability);
            Then(it_has_full_availablility);
            And(has_no_alerts);
        }

        [Test]
        public void a_set_of_available_and_unavailable_instances_are_partially_available()
        {
            Given(available_instance);
            And(() => unavailable_instance(unavailable_alert));
            When(getting_availability);
            Then(it_has_reduced_availablility);
        }

        [Test]
        public void a_set_of_available_and_unavailable_instances_alerts_those_that_are_down()
        {
            Given(available_instance);
            And(() => unavailable_instance(unavailable_alert));
            When(getting_availability);
            Then(() => alerts(AlertFactory.InstanceUnavailable(unavailable_alert.Monitor, unavailable_alert.Message)));
        }

        [Test]
        public void a_set_of_unavailable_instances_are_unavailable()
        {
            Given(() => unavailable_instance(unavailable_alert));
            And(() => unavailable_instance(unavailable_alert));
            When(getting_availability);
            Then(it_has_no_availablility);
        }

        [Test]
        public void a_set_of_unavailable_instances_alerts_that_all_instances_are_down()
        {
            Given(() => unavailable_instance(unavailable_alert));
            And(() => unavailable_instance(unavailable_alert_2));
            When(getting_availability);
            Then(() => alerts(AlertFactory.ServiceUnavailable(new MonitorInfo(unavailable_alert.Monitor.Type,
                    unavailable_alert.Monitor.Name,
                    unavailable_alert.Monitor.Instances.Concat(unavailable_alert_2.Monitor.Instances).ToArray()),
                unavailable_alert.Message)));
        }

        [Test]
        public void a_set_with_a_single_available_instance_alerts_redundancy_loss()
        {
            Given(available_instance);
            And(() => unavailable_instance(unavailable_alert));
            When(getting_availability);
            Then(() => alerts(
                AlertFactory.ServiceRedundancyLost(multi_instance_monitor.Info, "Redundancy has been lost.")));
        }
    }
}