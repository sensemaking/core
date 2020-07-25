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
                When(logging_timings);
                Then(it_is_a_metric);
                And(it_has_its_name);
                And(it_has_its_duration);
                And(it_has_any_additional_info);
            });
        }
    }
}