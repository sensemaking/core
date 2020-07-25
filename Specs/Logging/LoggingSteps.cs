using System;
using System.Serialization;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Sensemaking.Bdd;
using Serilog;

namespace Sensemaking.Http.Specs
{
    public partial class LoggingSpecs
    {
        private static readonly ILogger logger = Substitute.For<ILogger>();
        private Log to_log;
        private string was_logged;

        protected override void before_all()
        {
            base.before_all();
            Logging.Configure(logger);
        }

        protected override void before_each()
        {
            base.before_each();
            to_log = null;
            was_logged = null;
        }

        protected override void after_each()
        {
            base.after_each();
            logger.ClearSubstitute();
        }

        private void a_log()
        {
            to_log = new Log("Some interesting log.");
        }

        private void logging_information()
        {
            logger.When(l => l.Information(Arg.Any<string>())).Do(c => was_logged = c.Arg<string>());
            Logging.Information(to_log);
        }

        private void logging_warning()
        {
            logger.When(l => l.Warning(Arg.Any<string>())).Do(c => was_logged = c.Arg<string>());
            Logging.Warning(to_log);
        }

        private void logging_error()
        {
            logger.When(l => l.Error(Arg.Any<string>())).Do(c => was_logged = c.Arg<string>());
            Logging.Error(to_log);
        }

        private void logging_fatal()
        {
            logger.When(l => l.Fatal(Arg.Any<string>())).Do(c => was_logged = c.Arg<string>());
            Logging.Fatal(to_log);
        }

        private void it_logs_as_json()
        {
            was_logged.Deserialize<Log>().Message.should_be(to_log.Message);
        }
    }

    internal class Log
    {
        public Log(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}