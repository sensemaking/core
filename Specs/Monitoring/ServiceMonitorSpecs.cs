using NUnit.Framework;
using Sensemaking.Bdd;
using Sensemaking.Host.Monitoring;

namespace Sensemaking.Monitoring.Specs
{
    public partial class ServiceMonitorSpecs : Specification
    {
        [Test]
        public void it_provides_monitor_info()
        {
            Given(a_service_monitor);
            Then(it_has_its_monitor_info);
        }

        [Test]
        public void it_provides_overall_level_of_service_across_all_dependencies()
        {
            Given(a_service_monitor);
            And(dependency_1_is_available);
            And(dependency_2_is_unavailable);
            When(getting_service_availability);
            Then(overall_level_of_service_is_provided);
        }

        [Test]
        public void status_provides_monitoring_info_for_all_internal_monitors()
        {
            Given(a_service_monitor);
            And(dependency_1_is_available);
            And(dependency_2_is_unavailable);
            When(getting_status);
            Then(it_has_monitoring_info_for_each_dependency);
        }

        [Test]
        public void status_provides_alerts_from_internal_monitors()
        {
            Given(a_service_monitor);
            And(dependency_1_is_available);
            And(dependency_2_is_unavailable);
            When(getting_status);
            Then(it_has_any_monitored_alerts);
        }

        [Test]
        public void if_internal_monitors_indicate_availability_then_health_is_alive()
        {
            Given(a_service_monitor);
            And(dependency_1_is_available);
            When(getting_status);
            Then(() => health_is(ServiceMonitor.Status.Healthiness.Alive));
        }

        [Test]
        public void if_internal_monitors_indicate_no_availability_then_health_is_dead()
        {
            Given(a_service_monitor);
            And(dependency_2_is_unavailable);
            When(getting_status);
            Then(() => health_is(ServiceMonitor.Status.Healthiness.Dead));
        }

        [Test]
        public void if_internal_monitors_alert_without_being_down()
        {
            Given(a_service_monitor);
            And(dependency_1_has_instance_unavailability);
            When(getting_status);
            Then(() => health_is(ServiceMonitor.Status.Healthiness.Ill));
        }

        [Test]
        public void if_internal_monitors_indicate_redundancy_is_lost_then_health_is_on_last_legs()
        {
            Given(a_service_monitor);
            And(dependency_1_has_lost_redundancy);
            When(getting_status);
            Then(() => health_is(ServiceMonitor.Status.Healthiness.OnLastLegs));
        }
    }
}