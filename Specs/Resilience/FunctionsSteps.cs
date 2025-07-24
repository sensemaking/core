using System;
using System.Threading;
using NodaTime;
using Sensemaking.Bdd;
using Sensemaking.Resilience;

namespace Sensemaking.Specs
{
    public partial class FunctionsSpecs
    {
        private const string expected_mitigation_message = "test mitigation";
        private const string default_exception_message = "Function retry timed out.";
        private const string expected_exception_message = "test timeoutMessage";
        private const string the_exception_message = "Exception of type 'System.Exception' was thrown.";

        private int call_counter;
        private bool execution_result;
        private int fail_call_counter;
        private int aproximate_execution_count;
        private Func<bool> function;
        private IExecutable<bool> retry_wrapper;
        private Action<Exception> fail_action;
        private string mitigation_message;
        private const int number_of_attempts = 3;


        protected override void before_each()
        {
            base.before_each();

            call_counter = 0;
            fail_call_counter = 0;
            function = null;
            fail_action = (e) =>
            {
                fail_call_counter++;
            };
        }

        private void a_function()
        {
            function = () =>
            {
                call_counter++;
                SimulateWorkingProcess();
                return true;
            };
        }

        private void a_flaky_function_that_may_fail()
        {
            function = () =>
            {
                call_counter++;
                SimulateWorkingProcess();

                if (call_counter == 1)
                    throw new Exception();

                return true;
            };
        }

        private void a_flaky_function_that_may_return_unexpected_result()
        {
            function = () =>
            {
                call_counter++;
                SimulateWorkingProcess();

                if (call_counter == 1)
                    return false;

                return true;
            };
        }

        private void a_function_that_constantly_fails()
        {
            function = () =>
            {
                call_counter++;
                SimulateWorkingProcess();
                throw new Exception();
            };
        }

        private void a_function_returning_unexpected_result()
        {
            function = () =>
            {
                call_counter++;
                SimulateWorkingProcess();
                return false;
            };
        }

        private void with_retry_that_has_default_mitigation()
        {
            aproximate_execution_count = 3;
            retry_wrapper = function.KeepTrying()
                .Every(Period.FromMilliseconds(100))
                .For(Duration.FromMilliseconds(300))
                .UntilThereIsNoException();
        }

        private void with_retry_that_has_mitigation()
        {
            aproximate_execution_count = 3;
            retry_wrapper = function.KeepTrying()
                .Every(Period.FromMilliseconds(100))
                .For(Duration.FromMilliseconds(300))
                .UntilThereIsNoException()
                .IfItTimesOut(() => { mitigation_message = expected_mitigation_message; });
        }

        private void with_retry_that_handles_exceptions()
        {
            aproximate_execution_count = 3;
            retry_wrapper = function.KeepTrying()
                .Every(Period.FromMilliseconds(100))
                .For(Duration.FromMilliseconds(300))
                .UntilThereIsNoException()
                .IfItTimesOut(new Exception(expected_exception_message));
        }

        private void with_retry_that_handles_exception_messages()
        {
            aproximate_execution_count = 3;
            retry_wrapper = function.KeepTrying()
                .Every(Period.FromMilliseconds(100))
                .For(Duration.FromMilliseconds(300))
                .UntilThereIsNoException()
                .IfItTimesOut(expected_exception_message);
        }

        private void with_retry_that_has_success_condition()
        {
            aproximate_execution_count = 3;
            retry_wrapper = function.KeepTrying()
                .Every(Period.FromMilliseconds(100))
                .For(Duration.FromMilliseconds(300))
                .Until(result => result)
                .IfItTimesOut(expected_exception_message);
        }

        private void executing()
        {
            execution_result = retry_wrapper.Execute();
        }

        private void it_executes_on_the_first_attempt()
        {
            the_exception.should_be_null();
            call_counter.should_be(1);
            execution_result.should_be_true();
        }

        private void it_mitigates_after_last_attempt()
        {
            mitigation_message.should_be(expected_mitigation_message);
            call_counter.should_be_greater_than_or_equal_to(aproximate_execution_count - 1);
            call_counter.should_be_less_than_or_equal_to(aproximate_execution_count + 1);
        }

        private void it_reports_failure_after_last_attempt(string exception_message)
        {
            the_exception.Message.should_be(exception_message);
            call_counter.should_be_greater_than_or_equal_to(aproximate_execution_count - 1);
            call_counter.should_be_less_than_or_equal_to(aproximate_execution_count + 1);
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
            execution_result.should_be_true();
        }

        private void SimulateWorkingProcess()
        {
            Thread.Sleep(1);
        }

        private void with_retry_that_handles_exceptions_for_some_attempts()
        {
            retry_wrapper = function.KeepTrying()
                .Every(Period.FromMilliseconds(100))
                .For(number_of_attempts)
                .IfItFails(fail_action)
                .UntilThereIsNoException();
        }
    }
}
