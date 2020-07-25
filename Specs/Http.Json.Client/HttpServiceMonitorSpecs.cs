using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Http.Json.Client.Specs
{
    public partial class HttpServiceMonitorSpecs : Specification
    {
        [Test]
        public void is_available_when_service_is_up()
        {
            Given(the_service_is_up);
            When(checking_availability);
            Then(it_is_available);
        }

        [Test]
        public void is_not_available_when_service_is_down()
        {
            Given(the_service_is_down);
            When(checking_availability);
            Then(it_is_not_available);
        }

        [Test]
        public void provides_headers_to_service()
        {
            Given(headers);
            And(the_service_is_up);
            When(checking_availability);
            Then(the_headers_are_sent);
        }
    }
}