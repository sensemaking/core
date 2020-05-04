using NodaTime;
using NodaTime.Extensions;
using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Monitoring.Specs
{
    [TestFixture]
    public partial class AlertFactorySpecs : Specification
    {
        [Test]
        public void service_unavailable_is_a_0002()
        {
            var f = SystemClock.Instance.InZone(DateTimeZoneProviders.Tzdb["Europe/London"]).GetCurrentLocalDateTime();
            var w = SystemClock.Instance.InTzdbSystemDefaultZone().GetCurrentLocalDateTime();
            Given(a_service_unavailable_alert);
            Then(() => it_has_an_alert_code_of("0002"));
            And(it_has_the_monitor_it_was_created_with);
            And(it_has_the_message_it_is_created_with);
            And(it_is_a_type_of_alert);
        }

        [Test]
        public void service_redundancy_loss_is_a_0003()
        {
            Given(a_service_redundancy_lost_alert);
            Then(() => it_has_an_alert_code_of("0003"));
            And(it_has_the_monitor_it_was_created_with);
            And(it_has_the_message_it_is_created_with);
            And(it_is_a_type_of_alert);
        }

        [Test]
        public void component_instance_is_down_is_a_0004()
        {
            Given(an_instance_unavailable_alert);
            Then(() => it_has_an_alert_code_of("0004"));
            And(it_has_the_monitor_it_was_created_with);
            And(it_has_the_message_it_is_created_with);
            And(it_is_a_type_of_alert);
        }

        [Test]
        public void unknown_error_is_a_0005()
        {
            Given(an_unknown_error_alert);
            Then(() => it_has_an_alert_code_of("0005"));
            And(it_has_the_monitor_it_was_created_with);
            And(it_has_the_exception_message);
            And(it_has_the_exception_detail);
            And(it_has_the_additional_info_it_as_created_with);
            And(it_is_a_type_of_error);
        }

        [Test]
        public void information_alerts_can_be_raised()
        {
            Given(an_information_alert);
            Then(() => it_has_an_alert_code_of("info_alert"));
            And(it_has_the_monitor_it_was_created_with);
            And(it_has_the_informational_message);
            And(it_is_a_type_of_information);
        }
    }
}