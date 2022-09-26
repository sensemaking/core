using System;
using System.Serialization;
using System.Threading;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Sensemaking.Bdd;
using Sensemaking.Monitoring;
using Serilog;

namespace Sensemaking.Specs
{
    public partial class MetricSpecs
    {
        private static readonly MonitorInfo monitor_info = new MonitorInfo("Api", "Fake Api Monitor");
        private static readonly ILogger logger = Substitute.For<ILogger>();

        private static readonly int execution_time = 50;
        private const string name = "Bobby McGee";
        private static readonly FakeAdditionalInfo additional_info = new FakeAdditionalInfo("Some additional info.");
        private const string function_result = "The result";

        private Action action;
        private Metric<FakeAdditionalInfo> the_logged_metric;
        private Func<string> function;
        private string the_result;

        protected override void before_all()
        {
            base.before_all();
            Logging.Configure(monitor_info, logger);
        }

        protected override void before_each()
        {
            base.before_each();
            logger.When(l => l.Information(Arg.Any<string>())).Do(c => the_logged_metric = c.Arg<string>().Deserialize<Metric<FakeAdditionalInfo>>());
            action = null;
            function = null;
            the_logged_metric = null;
        }

        protected override void after_each()
        {
            base.after_each();
            logger.ClearSubstitute();
        }

        private void a_monitor_info() { }

        private void an_action()
        {
            action = () => Thread.Sleep(execution_time);
        }

        private void a_function()
        {
            function = () =>
            {
                Thread.Sleep(execution_time);
                return function_result;
            };
        }

        private void logging_action_timings()
        {
            Logging.TimeThis(action, name, additional_info);
        }

        private void logging_function_timings()
        {
            the_result = Logging.TimeThis(function, name, additional_info);
        }

        private void the_logging_monitor_is_logged()
        {
            the_logged_metric.Monitor.should_be(monitor_info);
        }

        private void a_metric_is_logged()
        {
            the_logged_metric.Type.should_be(LogEntryTypes.Metric);
        }

        private void its_name_is_logged()
        {
            the_logged_metric.Name.should_be(name);
        }

        private void the_duration_of_execution_is_logged()
        {
            the_logged_metric.Duration.should_be_greater_than(execution_time);
            the_logged_metric.Duration.should_be_less_than(execution_time + 50);
        }

        private void any_additional_info_is_logged()
        {
            the_logged_metric.Entry.Info.should_be(additional_info.Info);
        }

        private void the_funtion_result_is_provided()
        {
            the_result.should_be(function_result);
        }
    }

    internal class FakeAdditionalInfo
    {
        public FakeAdditionalInfo(string info)
        {
            Info = info;
        }

        public string Info { get; set; }
    }
}