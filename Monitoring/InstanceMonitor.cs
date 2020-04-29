using System;

namespace Sensemaking.Monitoring
{
    public abstract class InstanceMonitor : IMonitor
    {
        public MonitorInfo Info { get; }

        protected InstanceMonitor(MonitorInfo info)
        {
            if(info == MonitorInfo.Empty)
                throw new ArgumentException("Monitoring information must be provided.");

            Info = info;
        }

        public abstract Availability Availability();
    }
}