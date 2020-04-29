using System;
using System.Diagnostics;
using System.Serialization;
using slf4net;

namespace System.Logging
{
    public static class Logging
    {
        private static readonly ILogger Log = LoggerFactory.GetLogger("Logger");

        public static Action<object> Info = LogInfo;
        public static Action<object> Error = LogError;

        public static void On()
        {
            Info = LogInfo;
            Error = LogError;
        }

        public static void Off()
        {
            Info = logEntry => { };
            Error = logEntry => { };
        }
       
        public static void TimeThis(Action action, string name, object additionalInfo = null)
        {
            var timer = new Stopwatch();
            timer.Start();
            action();
            timer.Stop();

            Info(new { type = "Metric", name, duration = timer.Elapsed.TotalMilliseconds, additionalInfo });
        }

        public static T TimeThis<T>(Func<T> func, string name, object additionalInfo = null)
        {
            var timer = new Stopwatch();
            timer.Start();
            var response = func();
            timer.Stop();

            Info(new { type = "Metric", name, duration = timer.Elapsed.TotalMilliseconds, additionalInfo });

            return response;
        }

        private static void LogInfo(object logEntry) { Log.Info($"{logEntry.Serialize()}{Environment.NewLine}"); }

        private static void LogError(object logEntry) { Log.Error($"{logEntry.Serialize()}{Environment.NewLine}"); }
    }
}
