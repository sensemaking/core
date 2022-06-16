using System;

namespace Sensemaking.Monitoring
{
    public class Alert<T>
    {
        public Alert(string code, T alertInfo)
        {
            if (code.IsNullOrEmpty() || alertInfo == null)
                throw new ArgumentException("Alerts must have a code and some alert information.");

            Code = code;
            AlertInfo = alertInfo;
        }

        public string Code { get; }
        public T AlertInfo { get; }
        
        public override bool Equals(object obj)
        {
            if (!(obj is MonitoringAlert that))
                return false;

            return this.Code == that.Code && this.AlertInfo!.Equals(that.AlertInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, AlertInfo);
        }
    }

    public class MonitoringAlert : Alert<string>
    {
        public MonitorInfo Monitor { get; }

        protected internal MonitoringAlert(string code, MonitorInfo monitor, string message) : base(code, message)
        {
            if (monitor == MonitorInfo.Empty)
                throw new ArgumentException("Monitor alerts must have a code, monitor and message");

            Monitor = monitor;
        }
      
        public override bool Equals(object obj)
        {
            if (!(obj is MonitoringAlert that))
                return false;

            return base.Equals(that) && this.Monitor == that.Monitor;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Monitor);
        }
    }
}