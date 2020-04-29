using System;

namespace Sensemaking.Monitoring
{
    public class ServiceAvailabilityException : Exception
    {
        public ServiceAvailabilityException() : base("Service is currently unavailable.") { }
    }
}