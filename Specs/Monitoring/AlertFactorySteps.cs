using System;
using Sensemaking.Bdd;

namespace Sensemaking.Monitoring.Specs
{
    public partial class AlertFactorySpecs
    {
        private const string message = "Some exceedingly interesting message.";
        private static readonly Exception exception = new Exception(message);
        private static readonly object additional_info = new {cheese = "Gruyere"};
        private Alert alert;

        protected override void before_each()
        {
            base.before_each();
            alert = null;
        }

        private void a_service_unavailable_alert()
        {
            alert = AlertFactory.ServiceUnavailable(Fake.AnInstanceMonitor.Info, message);
        }

        private void a_service_redundancy_lost_alert()
        {
            alert = AlertFactory.ServiceRedundancyLost(Fake.AnInstanceMonitor.Info, message);
        }

        private void an_instance_unavailable_alert()
        {
            alert = AlertFactory.InstanceUnavailable(Fake.AnInstanceMonitor.Info, message);
        }

        private void an_unknown_error_alert()
        {
            alert = AlertFactory.UnknownErrorOccured(Fake.AnInstanceMonitor.Info, exception, additional_info);
        }

        private void an_information_alert()
        {
            alert = AlertFactory.InformationAlert("info_alert", Fake.AnInstanceMonitor.Info, message);
        }

        private void it_has_an_alert_code_of(string code)
        {
            alert.Code.should_be(code);
        }

        private void it_has_the_monitor_it_was_created_with()
        {
            alert.Monitor.should_be(Fake.AnInstanceMonitor.Info);
        }

        private void it_has_the_message_it_is_created_with()
        {
            alert.Message.should_be(message);
        }

        private void it_is_a_type_of_alert()
        {
            alert.Type.should_be("Alert");
        }

        private void it_has_the_exception_message()
        {
            alert.Message.should_be(exception.Message);
        }

        private void it_has_the_exception_detail()
        {
            (alert as ExceptionAlert).ExceptionDetail.should_be(exception.ToString());
        }

        private void it_has_the_additional_info_it_as_created_with()
        {
            (alert as ExceptionAlert).AdditionalInfo.should_be(additional_info);
        }

        private void it_is_a_type_of_error()
        {
            alert.Type.should_be("Error");
        }

        private void it_has_the_informational_message()
        {
            alert.Message.should_be(message);
        }

        private void it_is_a_type_of_information()
        {
            alert.Type.should_be("Information");
        }
    }
}