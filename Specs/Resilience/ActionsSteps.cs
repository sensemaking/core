using System;
using System.Threading;
using NodaTime;
using Sensemaking.Bdd;
using Sensemaking.Resilience;

namespace Sensemaking.Specs
{
    public partial class ActionsSpecs
    {
        private const string expected_exception_message = "test timeoutMessage";
        private const string the_exception_message = "Exception of type 'System.Exception' was thrown.";

        private int call_counter;
        private int fail_call_counter;
        private int approximate_execution_count;
        private const int number_of_attempts = 3;
        private Action action;
        private Action<Exception> fail_action;
        private IExecutable retry_wrapper;

        protected override void before_each()
        {
            base.before_each();

            call_counter = 0;
            fail_call_counter = 0;
            action = null;
            fail_action = (e) =>
            {
                fail_call_counter++;
            };
        }

        private void an_action()
        {
            action = () =>
            {
                call_counter++;
                SimulateWorkingProcess();
            };
        }

        private void a_flaky_action_that_may_fail()
        {
            action = () =>
            {
                call_counter++;
                SimulateWorkingProcess();

                if (call_counter == 1)
                    throw new Exception();
            };
        }

        private void an_action_that_constantly_fails()
        {
            action = () =>
            {
                call_counter++;
                SimulateWorkingProcess();
                throw new Exception();
            };
        }

        private void with_retry_that_handles_exceptions_for_a_period()
        {
            approximate_execution_count = 3;
            retry_wrapper = action.KeepTrying()
                .Every(Period.FromMilliseconds(100))
                .For(Duration.FromMilliseconds(300))
                .UntilThereIsNoException()
                .IfItTimesOut(expected_exception_message);
        }

        private void with_retry_that_handles_exceptions_for_some_attempts()
        {
            retry_wrapper = action.KeepTrying()
                .Every(Period.FromMilliseconds(100))
                .For(number_of_attempts)
                .IfItFails(fail_action)
                .UntilThereIsNoException();
        }

        private void executing()
        {
            retry_wrapper.Execute();
        }

        private void it_executes_on_the_first_attempt()
        {
            the_exception.should_be_null();
            call_counter.should_be(1);
        }

        private void it_reports_failure_after_last_attempt()
        {
            the_exception.Message.should_be(expected_exception_message);
            call_counter.should_be_greater_than_or_equal_to(approximate_execution_count - 1);
            call_counter.should_be_less_than_or_equal_to(approximate_execution_count + 1);
        }

        private void it_reports_the_failure_after_last_attempt()
        {
            the_exception.Message.should_be(the_exception_message);
            call_counter.should_be(number_of_attempts);
            fail_call_counter.should_be(number_of_attempts - 1);
        }

        private void it_executes_on_another_attempt()
        {
            the_exception.should_be_null();
            call_counter.should_be(2);
        }

        private void SimulateWorkingProcess()
        {
            Thread.Sleep(1);
        }
    }
}
