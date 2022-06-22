using System;
using System.Serialization;
using NSubstitute;
using Serilog;

namespace Sensemaking.Bdd
{
    public static class LoggingAssertions
    {
        public static void should_have_logged_as_information(this ILogger logger, object entry)
        {
            logger.Received().Information(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Log, entry).Serialize());
        }

        public static void should_have_logged_as_warning(this ILogger logger, object entry)
        {
            logger.Received().Warning(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Log, entry).Serialize());
        }

        public static void should_have_logged_as_error(this ILogger logger, object entry)
        {
            logger.Received().Error(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Log, entry).Serialize());
        }

        public static void should_have_logged_as_fatal(this ILogger logger, object entry)
        {
            logger.Received().Fatal(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Log, entry).Serialize());
        }

        public static void should_have_logged_as_information<T>(this ILogger logger, Alert<T> entry)
        {
            logger.Received().Information(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Alert, entry).Serialize());
        }

        public static void should_have_logged_as_warning<T>(this ILogger logger, Alert<T> entry)
        {
            logger.Received().Warning(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Alert, entry).Serialize());
        }

        public static void should_have_logged_as_error<T>(this ILogger logger, Alert<T> entry)
        {
            logger.Received().Error(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Alert, entry).Serialize());
        }

        public static void should_have_logged_as_fatal<T>(this ILogger logger, Alert<T> entry)
        {
            logger.Received().Fatal(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Alert, entry).Serialize());
        }
    }
}