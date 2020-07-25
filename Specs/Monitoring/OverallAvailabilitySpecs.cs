using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Monitoring.Specs
{
    public partial class OverallAvailabilitySpecs : Specification
    {
        [Test]
        public void two_available_services_have_full_service_and_combined_alerts()
        {
            Given(() => available_instance(instance_monitors_1));
            And(() => available_instance(instance_monitors_2));
            When(getting_overall_availability);
            Then(it_has_full_availability);
            And(combined_alerts);
        }

        [Test]
        public void available_service_and_reduced_service_have_reduced_overall_service_and_combined_alerts()
        {
            Given(() => available_instance(instance_monitors_1));
            And(() => available_instance(instance_monitors_2));
            And(() => unavailable_instance(instance_monitors_2));
            When(getting_overall_availability);
            Then(it_has_reduced_availability);
            And(combined_alerts);
        }

        [Test]
        public void available_service_and_unavailable_service_have_no_overall_service_and_combined_alerts()
        {
            Given(() => available_instance(instance_monitors_1));
            And(() => unavailable_instance(instance_monitors_2));
            When(getting_overall_availability);
            Then(it_has_no_availability);
            And(combined_alerts);
        }

        [Test]
        public void two_reduced_services_have_reduced_overall_service_and_combined_alerts()
        {
            Given(() => available_instance(instance_monitors_1));
            And(() => unavailable_instance(instance_monitors_1));
            And(() => available_instance(instance_monitors_2));
            And(() => unavailable_instance(instance_monitors_2));
            When(getting_overall_availability);
            Then(it_has_reduced_availability);
            And(combined_alerts);
        }

        [Test]
        public void reduced_service_and_available_service_have_reduced_overall_service_and_combined_alerts()
        {
            Given(() => available_instance(instance_monitors_1));
            And(() => unavailable_instance(instance_monitors_1));
            And(() => available_instance(instance_monitors_2));
            When(getting_overall_availability);
            Then(it_has_reduced_availability);
            And(combined_alerts);
        }

        [Test]
        public void reduced_service_and_unavailable_service_have_no_overall_service_and_combined_alerts()
        {
            Given(() => available_instance(instance_monitors_1));
            And(() => unavailable_instance(instance_monitors_1));
            And(() => unavailable_instance(instance_monitors_2));
            When(getting_overall_availability);
            Then(it_has_no_availability);
            And(combined_alerts);
        }

        [Test]
        public void two_unavailable_services_have_no_overall_service_and_combined_alerts()
        {
            Given(() => unavailable_instance(instance_monitors_1));
            And(() => unavailable_instance(instance_monitors_2));
            When(getting_overall_availability);
            Then(it_has_no_availability);
            And(combined_alerts);
        }

        [Test]
        public void unavailable_service_and_available_service_have_no_overall_service_and_combined_alerts()
        {
            Given(() => unavailable_instance(instance_monitors_1));
            And(() => available_instance(instance_monitors_2));
            When(getting_overall_availability);
            Then(it_has_no_availability);
            And(combined_alerts);
        }

        [Test]
        public void unavailable_service_and_reduced_service_have_no_overall_service_and_combined_alerts()
        {
            Given(() => unavailable_instance(instance_monitors_1));
            And(() => available_instance(instance_monitors_2));
            And(() => unavailable_instance(instance_monitors_2));
            When(getting_overall_availability);
            Then(it_has_no_availability);
            And(combined_alerts);
        }
    }
}