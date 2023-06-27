using Flurl;
using Flurl.Http;
using System;
using Sensemaking.Monitoring;

namespace Sensemaking.Http.Json.Client.Monitoring
{
    public class HttpServiceMonitor : InstanceMonitor
    {
        private readonly Access access;

        public HttpServiceMonitor(MonitorInfo info, Access access) : base(info)
        {
            this.access = access;
        }

        public override Availability Availability()
        {
            try
            {
                var request = new FlurlRequest(access.Url);
                access.Headers.ForEach(header => request.WithHeader(header.Name, header.Value));
                return request.GetAsync().Result.ResponseMessage.IsSuccessStatusCode ?
                    Sensemaking.Monitoring.Availability.Up() :
                    Sensemaking.Monitoring.Availability.Down(AlertFactory.ServiceUnavailable(Info, $"Service at {access.Url} is down."));
            }
            catch
            {
                return Sensemaking.Monitoring.Availability.Down(AlertFactory.ServiceUnavailable(Info, $"Service at {access.Url} is down."));
            }
        }
        
        public record Access(string Url, (string Name, string Value)[] Headers);
    }
}
