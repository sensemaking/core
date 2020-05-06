using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Monitoring.Specs
{
    [TestFixture]
    public partial class AvailabilitySpecs : Specification
    {
        private static readonly Alert unavailable_alert =
            AlertFactory.ServiceUnavailable(Fake.AnInstanceMonitor.Info, "It's down Bob");

        [Test]
        public void considers_available_as_having_full_availability()
        {
            Given(available);
            When(getting_availability);
            Then(it_is_fully_available);
            And(() => availability_is(true));
        }

        [Test]
        public void considers_unavailable_as_having_no_availability_with_alert_that_instance_is_down()
        {
            Given(() => unavailable(unavailable_alert));
            When(getting_availability);
            Then(it_is_unavailable);
            And(() => availability_is(false));
            And(() => alerts(unavailable_alert));
        }
    }
}