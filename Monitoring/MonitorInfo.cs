using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Sensemaking.Monitoring
{
    public struct MonitorInfo
    {
        public static MonitorInfo Empty = new MonitorInfo();

        public string Type { get; private set; }
        public string Name { get; private set; }
        public string[] Instances { get; private set; }

        internal MonitorInfo(MonitorInfo monitorInfo, params string[] instances) : this(monitorInfo.Type, monitorInfo.Name, instances) { }

        public MonitorInfo(string type, string name) : this(type, name, Environment.MachineName) { }

        public MonitorInfo(string type, string name, params string[] instances)
        {
            if (type.IsNullOrEmpty())
                throw new ArgumentException("Monitor type must be provided");

            Name = name;
            Type = type;
            Instances = instances.Where(x => !x.IsNullOrEmpty()).ToArray();
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
            return this.Type == that.Type && this.Name == that.Name && this.Instances.SequenceEqual(that.Instances);
        }

        public override bool Equals(object? that)
        {
            return that is MonitorInfo monitorInfo && Equals(monitorInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Name, Instances);
        }
    }
}