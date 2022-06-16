using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Monitoring.Specs
{
    public partial class AlertFactorySpecs : Specification
    {
        [Test]
        public void creates_service_unavailable_monitoring_alerts()
        {
            Given(a_service_unavailable_alert);
            Then(() => it_has_an_alert_name_of("ServiceUnavailable"));
            And(it_has_the_monitor_it_was_created_with);
            And(it_has_the_message_it_is_created_with);
        }

        [Test]
        public void creates_service_redundancy_loss_monitoring_alerts()
        {
            Given(a_service_redundancy_lost_alert);
            Then(() => it_has_an_alert_name_of("ServiceRedundancyLost"));
            And(it_has_the_monitor_it_was_created_with);
            And(it_has_the_message_it_is_created_with);
        }

        [Test]
        public void creates_component_instance_is_down_monitoring_alerts()
        {
            Given(an_instance_unavailable_alert);
            Then(() => it_has_an_alert_name_of("InstanceUnavailable"));
            And(it_has_the_monitor_it_was_created_with);
            And(it_has_the_message_it_is_created_with);
        }
    }
}