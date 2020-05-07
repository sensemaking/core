using System;
using Sensemaking.Host.Monitoring;

namespace Sensemaking.Monitoring.Specs
{
    internal class Fake
    {
        public static Monitor AMonitor = new Monitor();
        public static InstanceMonitor AnInstanceMonitor = new InstanceMonitor();
        public static InstanceMonitor AnAlertingInstanceMonitor(string alertMessage) => new InstanceMonitor(alertMessage);
        public static ServiceMonitor.Status AliveServiceMonitorStatus => new ServiceMonitor.Status(Availability.Up(), AMonitor.Info);
        public static ServiceMonitor.Status DeadServiceMonitorStatus => new ServiceMonitor.Status(Availability.Down(AlertFactory.InstanceUnavailable(AMonitor.Info, "Ooops")), AMonitor.Info);

        public static ServiceMonitor.Status IllServiceMonitorStatus => new ServiceMonitor.Status(Availability.Up() |
                                                                                                 Availability.Down(AlertFactory.InstanceUnavailable(AMonitor.Info, "Ooops")), AMonitor.Info);

        public static ServiceMonitor.Status OnLastLegsServiceMonitorStatus
        {
            get
            {
                var availability = Availability.Up();
                availability.Add(AlertFactory.ServiceRedundancyLost(AMonitor.Info, "Oooops"));
                return new ServiceMonitor.Status(availability, AMonitor.Info);
            }
        }

        internal class Monitor : IMonitor
        {
            public MonitorInfo Info { get; } = new MonitorInfo("FakeMonitor", "FakeMonitor", "FakeMonitorInstance");

            public Availability Availability()
            {
                throw new NotImplementedException();
            }
        }

        internal class InstanceMonitor : Monitoring.InstanceMonitor
        {
            public InstanceMonitor() : base(AMonitor.Info)
            {
                IsAvailable = true;
            }

            public InstanceMonitor(string alertMessage) : base(AMonitor.Info)
            {
                IsAvailable = false;
                Alert = AlertFactory.ServiceUnavailable(AMonitor.Info, alertMessage);
            }

            public override Availability Availability()
            {
                return IsAvailable ? Monitoring.Availability.Up() : Monitoring.Availability.Down(Alert);
            }

            private bool IsAvailable { get; set; }
            private Alert Alert { get; set; }
        }
    }
}