using System;
using System.Serialization;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.Core;
using Sensemaking.Bdd;
using Sensemaking.Monitoring;
using Serilog;

namespace Sensemaking.Http.Specs
{
    public partial class LoggingSpecs
    {
        private static readonly MonitorInfo monitor = new MonitorInfo("Api", "Fake Api Monitor");
        private static readonly ILogger logger = Substitute.For<ILogger>();

        private Action<CallInfo> capture_log;
        private FakeLog to_log;
        private FakeLog was_logged;

        protected override void before_all()
        {
            base.before_all();
            Logging.Configure(logger);
        }

        protected override void before_each()
        {
            base.before_each();
            capture_log = c => was_logged = c.Arg<string>().Deserialize<FakeLog>();
            to_log = null;
            was_logged = null;
        }

        protected override void after_each()
        {
            base.after_each();
            logger.ClearSubstitute();
        }

        private void a_monitor_info()
        {
        }

        private void a_log()
        {
            to_log = new FakeLog("Some interesting log.");
        }

        private void logging_information()
        {
            logger.When(l => l.Information(Arg.Any<string>())).Do(capture_log);
            Logging.Information(to_log);
        }

        private void logging_warning()
        {
            logger.When(l => l.Warning(Arg.Any<string>())).Do(capture_log);
            Logging.Warning(to_log);
        }

        private void logging_error()
        {
            logger.When(l => l.Error(Arg.Any<string>())).Do(capture_log);
            Logging.Error(to_log);
        }

        private void logging_fatal()
        {
            logger.When(l => l.Fatal(Arg.Any<string>())).Do(capture_log);
            Logging.Fatal(to_log);
        }

        private void it_logs_as_json()
        {
            was_logged.Message.should_be(to_log.Message);
        }
    }

    internal class FakeLog
    {
        public FakeLog(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}