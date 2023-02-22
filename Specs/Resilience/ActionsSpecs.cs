using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    [TestFixture]
    public partial class ActionsSpecs : Specification
    {
        [Test]
        public void action_executes_on_the_first_attempt_with_retry_on_exception()
        {
            Given(an_action);
            And(with_retry_that_handles_exceptions_for_a_period);
            When(executing);
            Then(it_executes_on_the_first_attempt);
        }
        
        [Test]
        public void faulty_action_reports_failure_after_last_retry_attempt()
        {
            scenario(() =>
            {
                Given(an_action_that_constantly_fails);
                And(with_retry_that_handles_exceptions_for_a_period);
                When(() => trying(executing));
                Then(it_reports_failure_after_last_attempt);
            });
            
            scenario(() =>
            {
                Given(an_action_that_constantly_fails);
                And(with_retry_that_handles_exceptions_for_some_attempts);
                When(() => trying(executing));
                Then(it_reports_the_failure_after_last_attempt);
            });
        }
        
        [Test]
        public void flaky_action_executes_after_several_attempts_with_retry_on_exception()
        {
            Given(a_flaky_action_that_may_fail);
            And(with_retry_that_handles_exceptions_for_a_period);
            When(executing);
            Then(it_executes_on_another_attempt);
        }
    }
}
