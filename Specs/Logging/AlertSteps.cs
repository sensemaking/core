using System;
using System.Serialization;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.Core;
using Sensemaking.Bdd;
using Sensemaking.Monitoring;
using Serilog;

namespace Sensemaking.Specs
{
    public partial class AlertSpecs
    {
        private static readonly MonitorInfo monitor_info = new MonitorInfo("Api", "Fake Api Monitor");
        private static readonly ILogger logger = Substitute.For<ILogger>();

        private Action<CallInfo> capture_log;
        private Alert<string> to_log;
        private LogEntry<Alert<string>> the_logged_alert;

        protected override void before_all()
        {
            base.before_all();
            Logging.Configure(monitor_info, logger);
        }

        protected override void before_each()
        {
            base.before_each();
            capture_log = c => the_logged_alert = c.Arg<string>().Deserialize<LogEntry<Alert<string>>>();
            to_log = null;
            the_logged_alert = null;
        }

        protected override void after_each()
        {
            base.after_each();
            logger.ClearSubstitute();
        }

        private void a_monitor_info() { }

        private void a_log()
        {
            to_log = new Alert<string>("Arrrrggggggghhh", "Bad alerty shizzle.");
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

        private void an_alert_is_logged()
        {
            the_logged_alert.Type.should_be(LogEntryTypes.Alert);
        }

        private void the_logging_monitor_is_logged()
        {
            the_logged_alert.Monitor.should_be(monitor_info);
        }

        private void its_name_is_logged()
        {
            the_logged_alert.Entry.Name.should_be(to_log.Name);
        }

        private void any_alert_information_is_logged()
        {
            the_logged_alert.Entry.AlertInfo.should_be(to_log.AlertInfo);
        }
    }
}