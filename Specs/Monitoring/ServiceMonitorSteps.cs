﻿using System.Collections.Generic;
using System.Linq;
using NodaTime;
using NSubstitute;
using Sensemaking.Bdd;
using Sensemaking.Host.Monitoring;

namespace Sensemaking.Monitoring.Specs
{
    public partial class ServiceMonitorSpecs
    {
        private IList<ServiceDependency> service_dependencies;
        private ServiceDependency service_dependency_1;
        private ServiceDependency service_dependency_2;
        private IMonitor dependency_1_monitor;
        private IMonitor dependency_2_monitor;
        private readonly MonitorInfo monitoring_info = new FakeMonitor().Info;
        private readonly Duration heartbeat = Duration.FromMilliseconds(100);
        private ServiceMonitor service_monitor;
        private Availability overall_availability;
        private ServiceMonitor.Status the_status;

        protected override void before_each()
        {
            base.before_each();
            service_dependencies = new List<ServiceDependency>();
            dependency_1_monitor = Substitute.For<IMonitor>();
            dependency_1_monitor.Info.Returns(new MonitorInfo("FakeDb", "Database", "Db1", "Db2"));

            dependency_2_monitor = Substitute.For<IMonitor>();
            dependency_2_monitor.Info.Returns(new MonitorInfo("FakeQueue", "Queue", "Queue1", "Queue3", "Queue4"));

            service_dependency_1 = new ServiceDependency(dependency_1_monitor);
            service_dependency_2 = new ServiceDependency(dependency_2_monitor);

            service_monitor = null;
            overall_availability = null;
            the_status = new ServiceMonitor.Status();
        }

        private void a_service_monitor()
        {
        }

        private void dependency_1_is_available()
        {
            service_dependency_1.Monitor.Availability().Returns(Availability.Up());
            service_dependencies.Add(service_dependency_1);
        }

        private void dependency_2_is_unavailable()
        {
            service_dependency_2.Monitor.Availability()
                .Returns(Availability.Down(AlertFactory.InstanceUnavailable(new FakeInstanceMonitor().Info, "Ooops")));
            service_dependencies.Add(service_dependency_2);
        }

        private void dependency_1_has_instance_unavailability()
        {
            service_dependency_1.Monitor.Availability().Returns(
                Availability.Up() | Availability.Up() |
                Availability.Down(AlertFactory.InstanceUnavailable(new FakeInstanceMonitor().Info, "Ooops"))
            );
            service_dependencies.Add(service_dependency_1);
        }

        private void dependency_1_has_lost_redundancy()
        {
            var availability = Availability.Up();
            availability.Add(AlertFactory.ServiceRedundancyLost(new FakeInstanceMonitor().Info, "Ooops"));
            service_dependency_1.Monitor.Availability().Returns(availability);
            service_dependencies.Add(service_dependency_1);
        }

        private void getting_service_availability()
        {
            service_monitor = new ServiceMonitor(monitoring_info, heartbeat, service_dependencies.ToArray());
            overall_availability = service_monitor.Availability();
        }

        private void getting_status()
        {
            service_monitor = new ServiceMonitor(monitoring_info, heartbeat, service_dependencies.ToArray());
            the_status = service_monitor.GetStatus();
        }

        private void it_has_monitoring_info()
        {
            service_monitor = new ServiceMonitor(monitoring_info, heartbeat, service_dependencies.ToArray());
            service_monitor.Info.Type.should_be(monitoring_info.Type);
            service_monitor.Info.Name.should_be(monitoring_info.Name);
            service_monitor.Info.Instances.should_be(monitoring_info.Instances);
        }

        private void it_has_its_monitoring_interval()
        {
            heartbeat.should_be(service_monitor.Heartbeat);
        }

        private void overall_level_of_service_is_provided()
        {
            var availability = dependency_1_monitor.Availability() & dependency_2_monitor.Availability();
            overall_availability.Status.should_be(availability.Status);
            overall_availability.Alerts.should_be(availability.Alerts);
        }

        private void it_has_its_monitoring_info_and_for_each_internal_monitor()
        {
            the_status.Monitor.should_be(service_monitor.Info);
            the_status.Monitoring.should_be(new[] {dependency_1_monitor.Info, dependency_2_monitor.Info});
        }

        private void it_has_any_monitored_alerts()
        {
            the_status.Alerts.should_be(service_monitor.Availability().Alerts);
        }

        private void health_is(ServiceMonitor.Status.Healthiness health)
        {
            the_status.Health.should_be(health);
        }
    }
}