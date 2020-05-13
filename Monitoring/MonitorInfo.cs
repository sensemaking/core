using System;
using System.Linq;

namespace Sensemaking.Monitoring
{
    public struct MonitorInfo
    {
        public static MonitorInfo Empty = new MonitorInfo();

        public string Type { get; }
        public string Name { get; }
        public string[] Instances { get; }

        internal MonitorInfo(MonitorInfo monitorInfo, params string[] instances) : this(monitorInfo.Type, monitorInfo.Name, instances) { }

        public MonitorInfo(string type, string name) : this(type, name, Environment.MachineName) { }

        public MonitorInfo(string type, string name, params string[] instances)
        {
            if (type.IsNullOrEmpty() || instances.None(x => !x.IsNullOrEmpty()))
                throw new ArgumentException("Monitor type and monitored instances must be provided");

            Name = name;
            Type = type;
            Instances = instances.ToArray();
        }

        public static bool operator ==(MonitorInfo @this, MonitorInfo that)
        {
            return @this.Equals(that);
        }

        public static bool operator !=(MonitorInfo @this, MonitorInfo that)
        {
            return !(@this == that);
        }

        public bool Equals(MonitorInfo that)
        {
            return string.Equals(Type, that.Type) && string.Equals(Name, that.Name) && (Instances ?? Array.Empty<string>()).SequenceEqual(that.Instances ?? Array.Empty<string>());
        }

        public override bool Equals(object that)
        {
            return that is MonitorInfo monitorInfo && Equals(monitorInfo);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type.GetHashCode();
                hashCode = (hashCode * 397) ^ Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Instances.GetHashCode();
                return hashCode;
            }
        }
    }
}