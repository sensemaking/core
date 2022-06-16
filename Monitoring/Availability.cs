using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sensemaking.Monitoring
{
    public class Availability
    {
        public State Status { get; }
        private List<MonitoringAlert> AlertList { get; } = new List<MonitoringAlert>();
        public MonitoringAlert[] Alerts => AlertList.ToArray();

        public static Availability Up()
        {
            return new Availability(State.Full);
        }

        public static Availability Down(MonitoringAlert downAlert)
        {
            return new Availability(State.None, downAlert);
        }

        private Availability(State state, params MonitoringAlert[] alerts)
        {
            Status = state;
            AlertList.AddRange(alerts);
        }

        public static implicit operator bool(Availability @this)
        {
            return @this.Status != State.None;
        }

        public static implicit operator State(Availability @this)
        {
            return @this.Status;
        }

        public static bool operator true(Availability @this)
        {
            return @this.Status != State.None;
        }

        public static bool operator false(Availability @this)
        {
            return @this.Status == State.None;
        }

        public static Availability operator &(Availability @this, Availability that)
        {
            var alerts = @this.Alerts.Concat(that.Alerts).ToArray();

            if (@this == State.None || that == State.None)
                return new Availability(State.None, alerts);

            if (@this == State.Reduced || that == State.Reduced)
                return new Availability(State.Reduced, alerts);

            return new Availability(State.Full, alerts);
        }

        public static Availability operator |(Availability @this, Availability that)
        {
            var newState = @this.Status == @that.Status ? @this.Status : State.Reduced;
            var alerts = Array.Empty<MonitoringAlert>();

            switch (newState)
            {
                case State.Reduced:
                    alerts = GetReducedAvailabilityAlerts(@this.AlertList.Concat(that.AlertList));
                    break;
                case State.None:
                    alerts = CollapseNoAvailabilityAlerts(@this.AlertList.Concat(that.AlertList));
                    break;
            }

            return new Availability(newState, alerts);
        }

        internal void Add(MonitoringAlert alert)
        {
            AlertList.Add(alert);
        }

        private static MonitoringAlert[] GetReducedAvailabilityAlerts(IEnumerable<MonitoringAlert> alerts)
        {
            return alerts.Select(x => AlertFactory.InstanceUnavailable(x.Monitor, x.AlertInfo)).ToArray();
        }

        private static MonitoringAlert[] CollapseNoAvailabilityAlerts(IEnumerable<MonitoringAlert> alerts)
        {
            var instances = alerts.SelectMany(x => x.Monitor.Instances).ToArray();
            return new [] { AlertFactory.ServiceUnavailable(new MonitorInfo(alerts.First().Monitor, instances), alerts.First().AlertInfo) };
        }

        public enum State
        {
            Full = 0,
            Reduced = 1,
            None = 2
        }
    }
}