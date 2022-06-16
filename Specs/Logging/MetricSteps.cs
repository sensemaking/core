using System;
using System.Serialization;
using System.Threading;
using NodaTime;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.Core;
using Sensemaking.Bdd;
using Sensemaking.Monitoring;
using Serilog;

namespace Sensemaking.Http.Specs
{
    public partial class MetricSpecs
    {
        private static readonly ILogger logger = Substitute.For<ILogger>();

        private static readonly int execution_time = 50;
        private const string name = "Bobby McGee";
        private static readonly FakeAdditionalInfo additional_info = new FakeAdditionalInfo("Some additional info.");
        private const string function_result = "The result";
            
        private Action action;
        private FakeMetricEntry the_logged_metric;
        private Func<string> function;
        private string the_result;

        protected override void before_all()
        {
            base.before_all();
            Logging.Configure(new MonitorInfo("Fake", "Fake"), logger);
        }

        protected override void before_each()
        {
            base.before_each();
            logger.When(l => l.Information(Arg.Any<string>())).Do(c => the_logged_metric = c.Arg<string>().Deserialize<FakeMetricEntry>());
            action = null;
            function = null;
            the_logged_metric = null;
        }

        protected override void after_each()
        {
            base.after_each();
            logger.ClearSubstitute();
        }

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

        private void a_metric_is_logged()
        {
            the_logged_metric.LogEntry.Type.should_be("Metric");
        }

        private void it_has_its_name()
        {
            the_logged_metric.LogEntry.Name.should_be(name);
        }

        private void it_has_the_duration_of_execution()
        {
            the_logged_metric.LogEntry.Duration.should_be_greater_than(execution_time);
            the_logged_metric.LogEntry.Duration.should_be_less_than(execution_time + 50);
        }

        private void it_has_any_additional_info()
        {
            the_logged_metric.LogEntry.AdditionalInfo.Info.should_be(additional_info.Info);
        }

        private void the_funtion_result_is_provided()
        {
            the_result.should_be(function_result);
        }
    }

    internal class FakeMetricEntry
    {
        public MonitorInfo Monitor { get; set; }
        public FakeMetric LogEntry { get; set; }
    }

    internal class FakeMetric
    {
        public FakeMetric(string type, string name, double duration, FakeAdditionalInfo additionalInfo)
        {
            Type = type;
            Name = name;
            Duration = duration;
            AdditionalInfo = additionalInfo;
        }

        public string Type { get; private set; }
        public string Name { get; private set; }
        public double Duration { get; private set; }
        public FakeAdditionalInfo AdditionalInfo { get; private set; }
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