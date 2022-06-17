using Flurl.Http.Testing;
using Sensemaking.Bdd;
using Sensemaking.Http.Json.Client.Monitoring;
using Sensemaking.Monitoring;

namespace Sensemaking.Specs
{
    public partial class HttpServiceMonitorSpecs
    {
        private HttpTest http;
        private object the_headers;
        private Availability the_availability;
        private readonly MonitorInfo info = new MonitorInfo("Service Monitor", "The Service");

        protected override void before_each()
        {
            base.before_each();
            http = new HttpTest();
            the_headers = null;
        }

        public void the_service_is_up()
        {
            http.RespondWith(status: 200);
        }

        public void the_service_is_down()
        {
            http.RespondWith(status: 503);
        }

        public void headers()
        {
            the_headers = new {Accept = "app/chocolate "};
        }

        public void checking_availability()
        {
            the_availability = new HttpServiceMonitor(info, "https://a_url", the_headers).Availability();
        }

        public void it_is_available()
        {
            the_availability.Status.should_be(Availability.Up().Status);
        }

        public void it_is_not_available()
        {
            var expected_availability = Availability.Down(AlertFactory.ServiceUnavailable(info, "Remote service is down."));
            the_availability.Status.should_be(expected_availability.Status);
        }

        public void the_headers_are_sent()
        {
            http.ShouldHaveMadeACall().WithHeader("Accept", "app/chocolate");
        }

        protected override void after_each()
        {
            base.after_each();
            http.Dispose();
        }
    }
}