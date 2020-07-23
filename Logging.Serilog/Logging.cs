using System;
using System.Runtime.CompilerServices;
using System.Serialization;
using Serilog;

namespace Sensemaking
{
    public static class Logging
    {
        public static void Configure(ILogger logger)
        {
            Log.Logger = logger;
        }

        public static void Information(object logEntry) => Log.Logger.Information(logEntry.Serialize());
        public static void Warning(object logEntry) => Log.Logger.Warning(logEntry.Serialize());
        public static void Error(object logEntry) => Log.Logger.Error(logEntry.Serialize());
        public static void Fatal(object logEntry) => Log.Logger.Fatal(logEntry.Serialize());

        public static void Information(this ILogger logger, object obj) { logger.Information(obj.Serialize()); }
        public static void Warning(this ILogger logger, object obj) { logger.Warning(obj.Serialize()); }
        public static void Error(this ILogger logger, object obj) { logger.Error(obj.Serialize()); }
        public static void Fatal(this ILogger logger, object obj) { logger.Fatal(obj.Serialize()); }
    }
}