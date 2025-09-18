using System;
using System.Serialization;
using NSubstitute;
using Serilog;

namespace Sensemaking.Bdd;

public static class LoggingAssertions
{
    public static void should_have_logged_information(this ILogger logger, object entry)
    {
        logger.Received().Information(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Log, entry).Serialize());
    }

    public static void should_have_logged_information<T>(this ILogger logger, Alert<T> entry)
    {
        logger.Received().Information(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Alert, entry).Serialize());
    }

    public static void should_have_logged_information(this ILogger logger)
    {
        logger.Received().Information(Arg.Any<string>());
    }

    public static void should_have_logged_warning(this ILogger logger, object entry)
    {
        logger.Received().Warning(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Log, entry).Serialize());
    }

    public static void should_have_logged_warning<T>(this ILogger logger, Alert<T> entry)
    {
        logger.Received().Warning(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Alert, entry).Serialize());
    }

    public static void should_have_logged_warning(this ILogger logger)
    {
        logger.Received().Warning(Arg.Any<string>());
    }

    public static void should_have_logged_error(this ILogger logger, object entry)
    {
        logger.Received().Error(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Log, entry).Serialize());
    }

    public static void should_have_logged_error<T>(this ILogger logger, Alert<T> entry)
    {
        logger.Received().Error(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Alert, entry).Serialize());
    }

    public static void should_have_logged_error(this ILogger logger)
    {
        logger.Received().Error(Arg.Any<string>());
    }

    public static void should_have_logged_fatal(this ILogger logger, object entry)
    {
        logger.Received().Fatal(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Log, entry).Serialize());
    }
        
    public static void should_have_logged_fatal<T>(this ILogger logger, Alert<T> entry)
    {
        logger.Received().Fatal(new LogEntry<object>(Logging.Monitor, LogEntryTypes.Alert, entry).Serialize());
    }

    public static void should_have_logged_fatal(this ILogger logger)
    {
        logger.Received().Fatal(Arg.Any<string>());
    }

    public static void should_not_have_logged_information(this ILogger logger)
    {
        logger.DidNotReceive().Information(Arg.Any<string>());
    }

    public static void should_not_have_logged_warning(this ILogger logger)
    {
        logger.DidNotReceive().Warning(Arg.Any<string>());
    }

    public static void should_not_have_logged_error(this ILogger logger)
    {
        logger.DidNotReceive().Error(Arg.Any<string>());
    }

    public static void should_not_have_logged_fatal(this ILogger logger)
    {
        logger.DidNotReceive().Fatal(Arg.Any<string>());
    }
}