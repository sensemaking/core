using System;
using System.Linq;
using NodaTime;
using Sensemaking.Monitoring;

namespace Sensemaking.Host.Monitoring
{
    public interface IMonitorServices
    {
        Period Heartbeat { get; }
        ServiceDependency[] Dependencies { get; }
        Availability Availability();
        ServiceMonitor.Status GetStatus();
    }

    public class ServiceMonitor : IMonitorServices
    {
        public Period Heartbeat { get; }
        public ServiceDependency[] Dependencies { get; }

        public ServiceMonitor(Period heartbeat, params ServiceDependency[] dependencies)
        {
            Heartbeat = heartbeat;
            Dependencies = dependencies;
        }

        public Availability Availability()
        {
            return Dependencies.Any() ? Dependencies.Select(x => x.Monitor.Availability()).Aggregate((x, y) => x & y) : Sensemaking.Monitoring.Availability.Up();
        }

        public Status GetStatus()
        {
            return new Status(Availability(), Dependencies.Select(x => x.Monitor.Info).ToArray());
        }
        
        public struct Status
        {
            public static readonly Status Empty = new Status();

            public MonitorInfo[] Monitoring { get; }
            public Alert[] Alerts { get; }
            public Healthiness Health { get; }

            internal Status(Availability availability, params MonitorInfo[] dependencyMonitors)
            {
                Monitoring = dependencyMonitors;
                Alerts = availability.Alerts;
                Health = GetHealth(availability);
            }

            private static Healthiness GetHealth(Availability availability)
            {
                if (availability)
                {
                    if (availability.Alerts.Any(alert => alert.Code == AlertFactory.Codes.ServiceRedundancyLost))
                        return Healthiness.OnLastLegs;

                    if (availability.Alerts.Any())
                        return Healthiness.Ill;

                    return Healthiness.Alive;
                }
                return Healthiness.Dead;
            }

            public enum Healthiness
            {
                Alive = 1,
                Ill = 2,
                OnLastLegs = 3,
                Dead = 4
            }

            public bool Equals(Status that)
            {
                return this.Monitoring == that.Monitoring && this.Alerts.SequenceEqual(that.Alerts);
            }

            public override bool Equals(object obj)
            {
                return obj is Status status && Equals(status);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Monitoring, Alerts);
            }
        }
    }
}