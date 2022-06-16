
namespace Sensemaking.Monitoring
{
    public class AlertFactory
    {
        public static MonitoringAlert InstanceUnavailable(MonitorInfo monitor, string message)
        {
            return new MonitoringAlert(monitor, MonitoringAlerts.InstanceUnavailable, message);
        }

        public static MonitoringAlert ServiceRedundancyLost(MonitorInfo monitor, string message)
        {
            return new MonitoringAlert(monitor, MonitoringAlerts.ServiceRedundancyLost, message);
        }

        public static MonitoringAlert ServiceUnavailable(MonitorInfo monitor, string message)
        {
            return new MonitoringAlert(monitor, MonitoringAlerts.ServiceUnavailable, message);
        }
    }
}