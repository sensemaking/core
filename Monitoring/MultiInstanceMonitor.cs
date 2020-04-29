using System;
using System.Collections.Generic;
using System.Linq;

namespace Sensemaking.Monitoring
{
    public class MultiInstanceMonitor : IMonitor
    {
        private readonly InstanceMonitor[] monitors;

        public MultiInstanceMonitor(IEnumerable<IMonitor> monitors)
        {
            if (!monitors.All(x => x is InstanceMonitor))
                throw new ArgumentException("Service monitors can only monitor across instance monitors.");

            this.monitors = monitors.Cast<InstanceMonitor>().ToArray();
            Info = new MonitorInfo($"{this.monitors.First().Info.Type}", this.monitors.First().Info.Name, this.monitors.SelectMany(x => x.Info.Instances).ToArray());
        }

        public MonitorInfo Info { get; }

        public Availability Availability()
        {
            var availability = monitors.Select(x => x.Availability()).Aggregate((x, y) => x | y);

            if (monitors.Count(x => x.Availability()) == 1)
                availability.Add(AlertFactory.ServiceRedundancyLost(Info, "Redundancy has been lost."));

            return availability;
        }
    }
}