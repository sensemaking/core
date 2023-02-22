using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    [TestFixture]
    public partial class FunctionsSpecs : Specification
    {
        [Test]
        public void function_executes_on_the_first_attempt_with_retry_on_exception()
        {
            Given(a_function);
            And(with_retry_that_handles_exceptions);
            When(executing);
            Then(it_executes_on_the_first_attempt);
        }
        
        [Test]
        public void function_executes_on_the_first_attempt_with_retry_on_success_condition()
        {
            Given(a_function);
            And(with_retry_that_has_success_condition);
            When(executing);
            Then(it_executes_on_the_first_attempt);
        }
        
        [Test]
        public void faulty_function_reports_failure_after_last_retry_attempt()
        {
            scenario(() =>
            {
                Given(a_function_that_constantly_fails);
                And(with_retry_that_handles_exceptions);
                When(() => trying(executing));
                Then(it_reports_failure_after_last_attempt);
            });
            
            scenario(() =>
            {
                Given(a_function_that_constantly_fails);
                And(with_retry_that_handles_exceptions_for_some_attempts);
                When(() => trying(executing));
                Then(it_reports_the_failure_after_last_attempt);
            });
        }
        
        [Test]
        public void function_returning_unexpected_result_reports_failure_after_last_retry_attempt()
        {
            Given(a_function_returning_unexpected_result);
            And(with_retry_that_has_success_condition);
            When(() => trying(executing));
            Then(it_reports_failure_after_last_attempt);
        }
        
        [Test]
        public void flaky_function_executes_after_several_attempts_with_retry_on_exception()
        {
            Given(a_flaky_function_that_may_fail);
            And(with_retry_that_handles_exceptions);
            When(executing);
            Then(it_executes_on_another_attempt);
        }
        
        [Test]
        public void flaky_function_executes_after_several_attempts_with_retry_on_success_condition()
        {
            Given(a_flaky_function_that_may_return_unexpected_result);
            And(with_retry_that_has_success_condition);
            When(executing);
            Then(it_executes_on_another_attempt);
        }
    }
}
