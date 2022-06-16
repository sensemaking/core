using System;
using System.Diagnostics;
using System.Serialization;
using Sensemaking.Monitoring;
using Serilog;

namespace Sensemaking
{
    public static class Logging
    {
        private static MonitorInfo monitor;

        public static void Configure(MonitorInfo monitor, ILogger logger)
        {
            Logging.monitor = monitor;
            Log.Logger = logger;
        }

        public static void Information(object logEntry) => Log.Logger.Information(logEntry);
        public static void Warning(object logEntry) => Log.Logger.Warning(logEntry);
        public static void Error(object logEntry) => Log.Logger.Error(logEntry);
        public static void Fatal(object logEntry) => Log.Logger.Fatal(logEntry);

        public static void Information(this ILogger logger, object obj) { logger.Information(obj.GetLogEntry()); }
        public static void Warning(this ILogger logger, object obj) { logger.Warning(obj.GetLogEntry()); }
        public static void Error(this ILogger logger, object obj) { logger.Error(obj.GetLogEntry()); }
        public static void Fatal(this ILogger logger, object obj) { logger.Fatal(obj.GetLogEntry()); }

        public static void TimeThis(Action action, string name, object? additionalInfo = null)
        {
            var timer = new Stopwatch();
            timer.Start();
            action();
            timer.Stop();

            Information(new { type = "Metric", name, duration = timer.Elapsed.TotalMilliseconds, additionalInfo });
        }

        public static T TimeThis<T>(Func<T> func, string name, object? additionalInfo = null)
        {
            var timer = new Stopwatch();
            timer.Start();
            var response = func();
            timer.Stop();

            Information(new { type = "Metric", name, duration = timer.Elapsed.TotalMilliseconds, additionalInfo });

            return response;
        }

        private static string GetLogEntry(this object log)
        {
            return new { Monitor = monitor, LogEntry = log }.Serialize();
        }
    }
}