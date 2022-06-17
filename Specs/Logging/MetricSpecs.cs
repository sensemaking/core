using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    public partial class MetricSpecs : Specification
    {
        [Test]
        public void logs_metrics_as_information()
        {
            scenario(() =>
            {
                Given(a_monitor_info);
                And(an_action);
                When(logging_action_timings);
                Then(the_logging_monitor_is_logged);
                And(a_metric_is_logged);
                And(its_name_is_logged);
                And(the_duration_of_execution_is_logged);
                And(any_additional_info_is_logged);
            });

            scenario(() =>
            {
                Given(a_monitor_info);
                And(a_function);
                When(logging_function_timings);
                Then(the_logging_monitor_is_logged);
                And(a_metric_is_logged);
                And(its_name_is_logged);
                And(the_duration_of_execution_is_logged);
                And(any_additional_info_is_logged);
                And(the_funtion_result_is_provided);
            });
        }
    }
}