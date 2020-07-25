using System;
using System.Serialization;
using System.Threading;
using NodaTime;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Sensemaking.Bdd;
using Serilog;

namespace Sensemaking.Http.Specs
{
    public partial class MetricSpecs
    {
        private static readonly ILogger logger = Substitute.For<ILogger>();
        private static readonly int execution_time = 50;
        private const string name = "Bobby McGee";
        private static readonly AdditionalInfo additional_info = new AdditionalInfo("Some additional info.");
            
        private Action action;
        private Metric logged_metric;
        
        protected override void before_all()
        {
            base.before_all();
            Logging.Configure(logger);
        }

        protected override void before_each()
        {
            base.before_each();
            action = null;
            logged_metric = null;
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

        private void logging_timings()
        {
            logger.When(l => l.Information(Arg.Any<string>())).Do(c => logged_metric = c.Arg<string>().Deserialize<Metric>());
            Logging.TimeThis(action, name, additional_info);
        }

        private void it_is_a_metric()
        {
            logged_metric.Type.should_be("Metric");
        }

        private void it_has_its_name()
        {
            logged_metric.Name.should_be(name);
        }

        private void it_has_its_duration()
        {
            logged_metric.Duration.should_be_greater_than(execution_time);
            logged_metric.Duration.should_be_less_than(execution_time + 50);
        }

        private void it_has_any_additional_info()
        {
            logged_metric.AdditionalInfo.Info.should_be(additional_info.Info);
        }
    }

    internal class Metric
    {
        public Metric(string type, string name, double duration, AdditionalInfo additionalInfo)
        {
            Type = type;
            Name = name;
            Duration = duration;
            AdditionalInfo = additionalInfo;
        }

        public string Type { get; private set; }
        public string Name { get; private set; }
        public double Duration { get; private set; }
        public AdditionalInfo AdditionalInfo { get; private set; }
    }

    internal class AdditionalInfo
    {
        public AdditionalInfo(string info)
        {
            Info = info;
        }

        public string Info { get; set; }
    }
}