using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Http.Specs
{
    public partial class MetricSpecs : Specification
    {
        [Test]
        public void logs_metrics_as_information()
        {
            scenario(() =>
            {
                Given(an_action);
                When(logging_action_timings);
                Then(a_metric_is_logged);
                And(it_has_its_name);
                And(it_has_the_duration_of_execution);
                And(it_has_any_additional_info);
            });

            scenario(() =>
            {
                Given(a_function);
                When(logging_function_timings);
                Then(a_metric_is_logged);
                And(it_has_its_name);
                And(it_has_the_duration_of_execution);
                And(it_has_any_additional_info);
                And(the_funtion_result_is_provided);
            });
        }
    }
}