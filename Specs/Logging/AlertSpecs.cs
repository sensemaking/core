using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    public partial class AlertSpecs : Specification
    {
        [Test]
        public void logs_information_alerts()
        {
            scenario(() =>
            {
                Given(a_monitor_info);
                And(an_alert);
                When(logging_information);
                Then(the_logging_monitor_is_logged);
                And(an_alert_is_logged);
                And(its_name_is_logged);
                And(any_alert_information_is_logged);
            });

            scenario(() =>
            {
                Given(a_monitor_info);
                And(a_monitoring_alert);
                When(logging_information);
                Then(the_logging_monitor_is_logged);
                And(an_alert_is_logged);
                And(its_name_is_logged);
                And(any_alert_information_is_logged);
            });
        }

        [Test]
        public void logs_warning_alerts()
        {
            scenario(() =>
            {
                Given(a_monitor_info);
                And(an_alert);
                When(logging_warnings);
                Then(the_logging_monitor_is_logged);
                And(an_alert_is_logged);
                And(its_name_is_logged);
                And(any_alert_information_is_logged);
            });

            scenario(() =>
            {
                Given(a_monitor_info);
                And(a_monitoring_alert);
                When(logging_warnings);
                Then(the_logging_monitor_is_logged);
                And(an_alert_is_logged);
                And(its_name_is_logged);
                And(any_alert_information_is_logged);
            });
        }

        [Test]
        public void logs_error_alerts()
        {
            scenario(() =>
            {
                Given(a_monitor_info);
                And(an_alert);
                When(logging_errors);
                Then(the_logging_monitor_is_logged);
                And(an_alert_is_logged);
                And(its_name_is_logged);
                And(any_alert_information_is_logged);
            });

            scenario(() =>
            {
                Given(a_monitor_info);
                And(a_monitoring_alert);
                When(logging_errors);
                Then(the_logging_monitor_is_logged);
                And(an_alert_is_logged);
                And(its_name_is_logged);
                And(any_alert_information_is_logged);
            });
        }

        [Test]
        public void logs_fatal_alerts()
        {
            scenario(() =>
            {
                Given(a_monitor_info);
                And(an_alert);
                When(logging_fatalities);
                Then(the_logging_monitor_is_logged);
                And(an_alert_is_logged);
                And(its_name_is_logged);
                And(any_alert_information_is_logged);
            });

            scenario(() =>
            {
                Given(a_monitor_info);
                And(a_monitoring_alert);
                When(logging_fatalities);
                Then(the_logging_monitor_is_logged);
                And(an_alert_is_logged);
                And(its_name_is_logged);
                And(any_alert_information_is_logged);
            });
        }
    }
}