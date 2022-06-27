using System;

namespace Sensemaking.Monitoring
{
    public static class MonitoringAlerts
    {
        public const string InstanceUnavailable = "InstanceUnavailable";
        public const string ServiceRedundancyLost = "ServiceRedundancyLost";
        public const string ServiceUnavailable = "ServiceUnavailable";
    }

    public class MonitoringAlert : Alert<string>
        {
            public MonitorInfo Monitor { get; }

            protected internal MonitoringAlert(MonitorInfo monitor, string name, string message) : base(name, message)
            {
                if(monitor == MonitorInfo.Empty)
                    throw new ArgumentException("Monitor alerts must have a code, monitor and message");

                Monitor = monitor;
            }

            public override bool Equals(object? obj)
            {
                if(obj is not MonitoringAlert that)
                    return false;

                return base.Equals(that) && this.Monitor == that.Monitor;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(base.GetHashCode(), Monitor);
            }
        }
    }