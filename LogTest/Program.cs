﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Serialization;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole;

namespace LogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //check rule of three
            //Discuss mocking.
            //Discuss fact that we cannot force the text formatter either.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new ObjectsAreJsonMessagesFormatter())
                .CreateLogger();

            //works statically and on ILogger instance - caller must remember for every log.
            Log.Information(new {Name = "Bob"}.Serialize());
            Log.Logger.Information(new {Name = "Bob"}.Serialize());

            //works on ILogger but can not be made available statically directly. Would have
            //to remeber to use Log.Logger or use DI based logger.
            Log.Logger.Information(new {Name = "Bob"});

            //puts sense|making in front and states static is the way to go. Other frameworks
            //will adapt to us (serilog in place now). Want to use Serilog directly? Go ahead
            //and do so. You can still use option 1 if you so desire.
            Logging.Information(new {Name = "Bob"});

            Console.WriteLine("Hello World!");
        }
    }

    public static class LoggerExtensions
    {
        public static void Information(this ILogger logger, object obj) { logger.Information(obj.Serialize()); }
        public static void Warning(this ILogger logger, object obj) { logger.Warning(obj.Serialize()); }
        public static void Error(this ILogger logger, object obj) { logger.Error(obj.Serialize()); }
        public static void Fatal(this ILogger logger, object obj) { logger.Fatal(obj.Serialize()); }
    }

    public static class Logging
    {
        public static void Information(object logEntry) => Log.Information(logEntry.Serialize());
        public static void Warning(object logEntry) => Log.Warning(logEntry.Serialize());
        public static void Error(object logEntry) => Log.Error(logEntry.Serialize());
        public static void Fatal(object logEntry) => Log.Fatal(logEntry.Serialize());
    }
}