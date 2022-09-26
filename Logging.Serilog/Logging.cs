using System;
using System.Diagnostics;
using System.Serialization;
using Sensemaking.Monitoring;
using Serilog;

namespace Sensemaking
{
    public static class Logging
    {
        internal static MonitorInfo Monitor;

        public static void Configure(MonitorInfo monitor, ILogger logger)
        {
            Logging.Monitor = monitor;
            Log.Logger = logger;
        }

        public static void Information(object logEntry) => Log.Logger.Information(logEntry);
        public static void Warning(object logEntry) => Log.Logger.Warning(logEntry);
        public static void Error(object logEntry) => Log.Logger.Error(logEntry);
        public static void Fatal(object logEntry) => Log.Logger.Fatal(logEntry);

        public static void Information(this ILogger logger, object obj) { logger.WriteInformation(obj.GetLogEntry()); }
        public static void Warning(this ILogger logger, object obj) { logger.WriteWarning(obj.GetLogEntry()); }
        public static void Error(this ILogger logger, object obj) { logger.WriteError(obj.GetLogEntry()); }
        public static void Fatal(this ILogger logger, object obj) { logger.WriteFatal(obj.GetLogEntry()); }

        private static void WriteInformation(this ILogger logger, LogEntry<object?> obj) { logger.Information(obj.Serialize()); }
        private static void WriteWarning(this ILogger logger, LogEntry<object?> obj) { logger.Warning(obj.Serialize()); }
        private static void WriteError(this ILogger logger, LogEntry<object?> obj) { logger.Error(obj.Serialize()); }
        private static void WriteFatal(this ILogger logger, LogEntry<object?> obj) { logger.Fatal(obj.Serialize()); }

        public static void TimeThis(Action action, string name, object? additionalInfo = null)
        {
            var timer = new Stopwatch();
            timer.Start();
            action();
            timer.Stop();

            Log.Logger.WriteInformation(new Metric<object?>(Monitor, name, timer.Elapsed.TotalMilliseconds, additionalInfo));
        }

        public static T TimeThis<T>(Func<T> func, string name, object? additionalInfo = null)
        {
            var timer = new Stopwatch();
            timer.Start();
            var response = func();
            timer.Stop();

            Log.Logger.WriteInformation(new Metric<object?>(Monitor, name, timer.Elapsed.TotalMilliseconds, additionalInfo));
            return response;
        }

        private static LogEntry<object?> GetLogEntry(this object entry)
        {
            return new LogEntry<object?>(Monitor, entry.IsAlert() ? LogEntryTypes.Alert : LogEntryTypes.Log, entry);
        }
        
        private static bool IsAlert(this object entry)
        {
            var type = entry.GetType();
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Alert<>))
                    return true;
        
                type = type.BaseType;
            }
            return false;
        }
    }

    internal class LogEntry<T>
    {
        public LogEntry(MonitorInfo monitor, LogEntryTypes type, T entry)
        {
            Monitor = monitor;
            Type = type;
            Entry = entry;
        }

        public MonitorInfo Monitor { get; private set; }
        public LogEntryTypes Type { get; private set; }
        public T Entry { get; private set; }
    }

    internal class Metric<T> : LogEntry<T>
    {
        public Metric(MonitorInfo monitor, string name, double durationInMilliseconds, T additionalInfo) : base(monitor, LogEntryTypes.Metric, additionalInfo)
        {
            Name = name;
            Duration = durationInMilliseconds;
        }

        public string Name { get; private set; }
        public double Duration { get; private set; }
    }

        public enum LogEntryTypes
        {
            Log = 0,
            Alert = 1,
            Metric = 2
        }
}