using System;

namespace Sensemaking.Monitoring
{
    public class ExceptionAlert : Alert
    {
        public object AdditionalInfo { get; }
        public string ExceptionDetail { get; }

        public ExceptionAlert(string code, MonitorInfo monitor, Exception ex, object additionalInfo = null) : base(code, monitor, ex.Message, Types.Error)
        {
            if (ex == null)
                throw new ArgumentException("ExceptionAlerts must have an exception");

            AdditionalInfo = additionalInfo;
            ExceptionDetail = ex.ToString();
        }
      
        public override bool Equals(object obj)
        {
            if (!(obj is ExceptionAlert that))
                return false;

            return base.Equals(obj) && this.AdditionalInfo == that.AdditionalInfo && this.ExceptionDetail == that.ExceptionDetail;
        }

        public override int GetHashCode()
        {
            var hash = base.GetHashCode();
            hash = hash * 23 + this.AdditionalInfo.GetHashCode();
            hash = hash * 23 + this.ExceptionDetail.GetHashCode();
            return hash;
        }
    }
}