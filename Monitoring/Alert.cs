using System;

namespace Sensemaking.Monitoring
{
    public class Alert
    {
        public string Code { get; }
        public MonitorInfo Monitor { get; }
        public string Message { get; }

        protected internal Alert(string code, MonitorInfo monitor, string message)
        {
            if (code.IsNullOrEmpty() || monitor == MonitorInfo.Empty || message.IsNullOrEmpty())
                throw new ArgumentException("Alerts must have a code, monitor and message");

            Code = code;
            Monitor = monitor;
            Message = message;
        }
      
        public override bool Equals(object obj)
        {
            if (!(obj is Alert that))
                return false;

            return this.Monitor == that.Monitor && this.Code == that.Code && this.Message == that.Message;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + this.Monitor.GetHashCode();
            hash = hash * 23 + this.Code.GetHashCode();
            hash = hash * 23 + this.Message.GetHashCode();
            return hash;
        }
    }
}