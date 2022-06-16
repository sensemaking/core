using System;

namespace Sensemaking.Monitoring
{
    public class AlertFactory
    {
        public class Codes
        {
            public const string ServiceUnavailable = "0002";
            public const string ServiceRedundancyLost = "0003";
            public const string InstanceUnavailable = "0004";
            public const string UnknownErrorOccured = "0005";
        }

        public static MonitoringAlert ServiceUnavailable(MonitorInfo monitor, string message)
        {
            return new MonitoringAlert(Codes.ServiceUnavailable, monitor, message);
        }

        public static MonitoringAlert ServiceRedundancyLost(MonitorInfo monitor, string message)
        {
            return new MonitoringAlert(Codes.ServiceRedundancyLost, monitor, message);
        }
        
        public static MonitoringAlert InstanceUnavailable(MonitorInfo monitor, string message)
        {
            return new MonitoringAlert(Codes.InstanceUnavailable, monitor, message);
        }

        public static MonitoringAlert UnknownErrorOccured(MonitorInfo monitor, Exception exception, object? additionalInfo = null)
        {
            return new ExceptionAlert(Codes.UnknownErrorOccured, monitor, exception, additionalInfo);
        }
    }
}