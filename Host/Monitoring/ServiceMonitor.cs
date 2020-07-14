﻿using System.Linq;
using NodaTime;
using Sensemaking.Monitoring;

namespace Sensemaking.Host.Monitoring
{
    public interface IMonitorServices
    {
        ServiceMonitor.Status GetStatus();
        Period Heartbeat { get; }
        Availability Availability();
    }

    public class ServiceMonitor : IMonitorServices
    {
        private readonly ServiceDependency[] dependencies;
        public Period Heartbeat { get; }

        public ServiceMonitor(Period heartbeat, params ServiceDependency[] dependencies)
        {
            Heartbeat = heartbeat;
            this.dependencies = dependencies;
        }

        public Availability Availability()
        {
            return dependencies.Any() ? dependencies.Select(x => x.Monitor.Availability()).Aggregate((x, y) => x & y) : Sensemaking.Monitoring.Availability.Up();
        }

        public Status GetStatus()
        {
            return new Status(Availability(), dependencies.Select(x => x.Monitor.Info).ToArray());
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

            public bool Equals(Status other)
            {
                return Equals(Monitoring, other.Monitoring) && Equals(Alerts, other.Alerts);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is Status && Equals((Status) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Monitoring.GetHashCode();
                    hashCode = (hashCode * 397) ^ Alerts.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}