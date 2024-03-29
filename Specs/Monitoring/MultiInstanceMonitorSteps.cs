﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sensemaking.Bdd;
using Sensemaking.Monitoring;

namespace Sensemaking.Specs
{
    public partial class MultiInstanceMonitorSpecs
    {
        private IList<IMonitor> instance_monitors;
        private Availability service_availability;
        private MultiInstanceMonitor multi_instance_monitor;

        protected override void before_each()
        {
            base.before_each();
            service_availability = null;
            instance_monitors = new List<IMonitor>();
            multi_instance_monitor = null;
        }

        private void a_non_instance_monitor()
        {
            instance_monitors.Add(Fake.AMonitor);
        }

        private void available_instance()
        {
            instance_monitors.Add(Fake.AnInstanceMonitor);
        }

        private void unavailable_instance(MonitoringAlert alert)
        {
            instance_monitors.Add(Fake.AnAlertingInstanceMonitor(alert.AlertInfo));
        }

        public void getting_availability()
        {
            multi_instance_monitor = new MultiInstanceMonitor(instance_monitors);
            service_availability = multi_instance_monitor.Availability();
        }
    
        private void multi_instance_monitor_cannot_be_used()
        {
            trying(() => new MultiInstanceMonitor(instance_monitors));
            informs<ArgumentException>("Service monitors can only monitor across instance monitors.");
        }

        private void monitor_info_has_name_and_type()
        {
            multi_instance_monitor = new MultiInstanceMonitor(instance_monitors);
            var monitor_info = multi_instance_monitor.Info;
            monitor_info.Type.should_be($"{instance_monitors.First().Info.Type}");
            monitor_info.Name.should_be(instance_monitors.First().Info.Name);
        }

        private void lists_all_monitored_instances()
        {
            new MultiInstanceMonitor(instance_monitors).Info.Instances.should_be(
                instance_monitors.SelectMany(x => x.Info.Instances));
        }

        private void it_has_full_availablility()
        {
            service_availability.Status.should_be(Availability.State.Full);
        }

        private void it_has_no_availablility()
        {
            service_availability.Status.should_be(Availability.State.None);
        }

        private void it_has_reduced_availablility()
        {
            service_availability.Status.should_be(Availability.State.Reduced);
        }

        private void alerts(MonitoringAlert alert)
        {
            service_availability.Alerts.should_contain(alert);
        }

        private void has_no_alerts()
        {
            service_availability.Alerts.should_be_empty();
        }
    }
}