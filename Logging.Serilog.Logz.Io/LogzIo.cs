using System;
using Serilog;
using Serilog.Sinks.Logz.Io;

namespace Sensemaking
{
    public static class LogzIo
    {
        public static ILogger CreateLogger(Settings settings)
        {
            return new LoggerConfiguration()
                .WriteTo.LogzIoEcs(new LogzioEcsOptions
                {
                    Type = settings.Type.ToString().ToLower(),
                    AuthToken = settings.Token,
                    DataCenter = new LogzioDataCenter { SubDomain = settings.SubDomain, Port = settings.Port, UseHttps = true }
                }, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();
        }


        public record Settings(string SubDomain, int Port, string Token, LogzType Type);

        public enum LogzType
        {
            Local = 1,
            Development = 2,
            Staging = 3,
            Production = 4
        }
    }

}