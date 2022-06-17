using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    public partial class AlertSpecs : Specification
    { 
        [Test]
        public void logs_information_alerts_as_json()
        {
            Given(a_monitor_info);
            And(a_log);
            When(logging_information);
            Then(the_logging_monitor_is_logged);
            And(an_alert_is_logged);
            And(its_name_is_logged);
            And(any_alert_information_is_logged);
        }
 
        [Test]
        public void logs_warning_alerts_as_json()
        {
            Given(a_monitor_info);
            And(a_log);
            When(logging_warning);
            Then(the_logging_monitor_is_logged);
            And(an_alert_is_logged);
            And(its_name_is_logged);
            And(any_alert_information_is_logged);
        }

        [Test]
        public void logs_error_alerts_as_json()
        {
            Given(a_monitor_info);
            And(a_log);
            When(logging_error);
            Then(the_logging_monitor_is_logged);
            And(an_alert_is_logged);
            And(its_name_is_logged);
            And(any_alert_information_is_logged);
        }

        [Test]
        public void logs_fatal_alerts_as_json()
        {
            Given(a_monitor_info);
            And(a_log);
            When(logging_fatal);
            Then(the_logging_monitor_is_logged);
            And(an_alert_is_logged);
            And(its_name_is_logged);
            And(any_alert_information_is_logged);
        }
    }
}