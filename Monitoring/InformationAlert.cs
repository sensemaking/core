using System;

namespace Sensemaking.Monitoring
{
    public class InformationAlert : Alert
    {
        public InformationAlert(string code, MonitorInfo monitor, string message) : base(code, monitor, message, Types.Information)
        {
            if (message.IsNullOrEmpty())
                throw new ArgumentException("InformationAlerts must have a message");
        }
      
        public override bool Equals(object obj)
        {
            if (!(obj is InformationAlert that))
                return false;

            return base.Equals(obj) && this.Message == that.Message;
        }

        public override int GetHashCode()
        {
            var hash = base.GetHashCode();
            hash = hash * 24 + this.Message.GetHashCode();
            return hash;
        }
    }
}