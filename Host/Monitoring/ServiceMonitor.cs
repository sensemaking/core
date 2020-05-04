using System.Linq;
using NodaTime;
using Sensemaking.Monitoring;

namespace Sensemaking.Host.Monitoring
{
    public interface IMonitorServices : IMonitor
    {
        ServiceMonitor.Status GetStatus();
        Duration Heartbeat { get; }
    }

    public class ServiceMonitor : IMonitorServices
    {
        private readonly ServiceDependency[] dependencies;
        public MonitorInfo Info { get; }
        public Duration Heartbeat { get; }

        public ServiceMonitor(MonitorInfo info, Duration heartbeat, params ServiceDependency[] dependencies)
        {
            this.dependencies = dependencies;
            Info = info;
            Heartbeat = heartbeat;
        }

        public Availability Availability()
        {
            return dependencies.Any() ? dependencies.Select(x => x.Monitor.Availability()).Aggregate((x, y) => x & y) : Sensemaking.Monitoring.Availability.Up();
        }

        public Status GetStatus()
        {
            return new Status(Info, dependencies.Select(x => x.Monitor.Info).ToArray(), Availability());
        }
        
        public struct Status
        {
            public static readonly Status Empty = new Status();

            public MonitorInfo Monitor { get; }
            public MonitorInfo[] Monitoring { get; }
            public Alert[] Alerts { get; }
            public Healthiness Health { get; }

            internal Status(MonitorInfo monitor, MonitorInfo[] dependencyMonitors, Availability availability)
            {
                Monitor = monitor;
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
                return Monitor.Equals(other.Monitor) && Equals(Monitoring, other.Monitoring) && Equals(Alerts, other.Alerts);
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
                    var hashCode = Monitor.GetHashCode();
                    hashCode = (hashCode * 397) ^ (Monitoring != null ? Monitoring.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Alerts != null ? Alerts.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }
    }
}