using System;

namespace Sensemaking
{
    public class Alert
    {
        public Alert(string code, object? alertInfo = null)
        {
            if (code.IsNullOrEmpty())
                throw new ArgumentException("Alerts must have a code.");

            Code = code;
            AlertInfo = alertInfo;
        }

        public string Code { get; }
        public object? AlertInfo { get; }
        
        public override bool Equals(object obj)
        {
            if (!(obj is Alert that))
                return false;

            return this.Code == that.Code && this.AlertInfo == that.AlertInfo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, AlertInfo);
        }
    }

}