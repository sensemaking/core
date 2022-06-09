using Flurl.Http;
using Sensemaking.Monitoring;

namespace Sensemaking.Http.Json.Client.Monitoring
{
    public class HttpServiceMonitor : InstanceMonitor
    {
        private readonly string url;
        private readonly object headers;

        public HttpServiceMonitor(MonitorInfo info, string url, object headers) : base(info)
        {
            this.url = url;
            this.headers = headers;
        }

        public override Availability Availability()
        {
            try
            {
                return url.WithHeaders(headers).GetAsync().Result.ResponseMessage.IsSuccessStatusCode ?
                    Sensemaking.Monitoring.Availability.Up() :
                    Sensemaking.Monitoring.Availability.Down(AlertFactory.ServiceUnavailable(Info, $"Service at {url} is down."));
            }
            catch
            {
                return Sensemaking.Monitoring.Availability.Down(AlertFactory.ServiceUnavailable(Info, $"Service at {url} is down."));
            }
        }
    }
}
