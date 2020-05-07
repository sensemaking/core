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
            public const string UiErrorOccurred = "0098";
        }

        public static Alert ServiceUnavailable(MonitorInfo monitor, string message)
        {
            return new Alert(Codes.ServiceUnavailable, monitor, message);
        }

        public static Alert ServiceRedundancyLost(MonitorInfo monitor, string message)
        {
            return new Alert(Codes.ServiceRedundancyLost, monitor, message);
        }
        
        public static Alert InstanceUnavailable(MonitorInfo monitor, string message)
        {
            return new Alert(Codes.InstanceUnavailable, monitor, message);
        }

        public static Alert UnknownErrorOccured(MonitorInfo monitor, Exception exception, object? additionalInfo = null)
        {
            return new ExceptionAlert(Codes.UnknownErrorOccured, monitor, exception, additionalInfo);
        }

        public static Alert InformationAlert(string code, MonitorInfo monitor, string message)
        {
            return new InformationAlert(code, monitor, message);
        }
    }
}