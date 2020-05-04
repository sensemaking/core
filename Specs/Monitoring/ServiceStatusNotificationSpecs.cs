using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Monitoring.Specs
{
    [TestFixture]
    public partial class ServiceStatusNotificationSpecs : Specification
    {
        [Test]
        public void sends_a_status_update_according_to_its_heartbeat_interval()
        { 
            Given(a_service_monitor);
            And(a_heartbeat_interval);
            When(notifying_of_service_status);
            Then(status_is_logged);
            And(it_is_logged_again_after_heartbeat_interval);
        }
    }
}