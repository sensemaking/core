using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Monitoring.Specs
{
    [TestFixture]
    public partial class ServiceStatusNotificationSpecs : Specification
    {
        [Test]
        public void logs_status_according_to_service_monitors_heartbeat_interval()
        {
            Given(a_service_monitor);
            And(it_has_a_status);
            And(a_heartbeat_interval);
            When(notifying_of_service_status);
            Then(status_is_logged);
            And(it_is_logged_again_after_heartbeat_interval);
        }

        [Test]
        public void logs_status_as_information_its_health_is_alive()
        {
            Given(a_service_monitor);
            And(its_status_is_alive);
            When(notifying_of_service_status);
            Then(status_is_logged_as_information);
        }

        [Test]
        public void logs_status_as_warning_when_its_health_is_ill()
        {
            Given(a_service_monitor);
            And(its_status_is_ill);
            When(notifying_of_service_status);
            Then(status_is_logged_as_a_warning);
        }

        [Test]
        public void logs_status_as_error_when_its_health_is_on_last_legs()
        {
            Given(a_service_monitor);
            And(its_status_is_on_last_legs);
            When(notifying_of_service_status);
            Then(status_is_logged_as_an_error);
        }

        [Test]
        public void logs_status_as_fatal_when_its_health_is_dead()
        {
            Given(a_service_monitor);
            And(its_status_is_dead);
            When(notifying_of_service_status);
            Then(status_is_logged_as_fatal);
        }
    }
}