using System;
using System.Threading;
using System.Threading.Tasks;
using NodaTime;
using Sensemaking.Bdd;
using Sensemaking.Resilience;

namespace Sensemaking.Specs
{
    public partial class AsyncActionsSpecs
    {
        private const string expected_exception_message = "The delegate executed asynchronously through TimeoutPolicy did not complete within the timeout.";
        private const string the_exception_message = "Exception of type 'System.Exception' was thrown.";

        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1,1);
        private int call_counter;
        private int fail_call_counter;
        private int aproximate_execution_count;
        private const int number_of_attempts = 3;
        private Func<Task> async_action;
        private Action<Exception> fail_action;
        private IAsyncExecutable retry_wrapper;

        protected override void before_each()
        {
            base.before_each();

            call_counter = 0;
            fail_call_counter = 0;
            async_action = null;
            fail_action = (e) =>
            {
                fail_call_counter++;
            };
        }

        private void an_async_action()
        {
            async_action = async () =>
            {
                    call_counter++;
                    await SimulateWorkingProcessAsync();
            };
        }

        private void a_flaky_async_action_that_may_fail()
        {
            async_action = async () =>
            {
                    call_counter++;
                    await SimulateWorkingProcessAsync();

                    if (call_counter == 1)
                        throw new Exception();
            };
        }

        private void an_async_action_that_constantly_fails()
        {
            async_action = async () =>
            {
                    call_counter++;
                    await SimulateWorkingProcessAsync();
                    throw new Exception();
            };
        }

        private void with_retry_that_handles_exceptions_for_a_period()
        {
            aproximate_execution_count = 3;
            retry_wrapper = async_action.KeepTryingAsync()
                .Every(Period.FromMilliseconds(100))
                .For(Duration.FromMilliseconds(300))
                .UntilThereIsNoException()
                .IfItTimesOutAsync(expected_exception_message);
        }

        private void with_retry_that_handles_exceptions_for_some_attempts()
        {
            retry_wrapper = async_action.KeepTryingAsync()
                .Every(Period.FromMilliseconds(100))
                .For(number_of_attempts)
                .IfItFails(fail_action)
                .UntilThereIsNoExceptionAsync();
        }

        private void executing_async()
        {
            retry_wrapper.ExecuteAsync().GetAwaiter().GetResult();
        }

        private void it_executes_on_the_first_attempt()
        {
            the_exception.should_be_null();
            call_counter.should_be(1);
        }

        private void it_reports_failure_after_last_attempt()
        {
            the_exception.Message.should_be(the_exception_message);
            call_counter.should_be_greater_than_or_equal_to(aproximate_execution_count-1);
            call_counter.should_be_less_than_or_equal_to(aproximate_execution_count+1);
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

        private async Task SimulateWorkingProcessAsync()
        {
            await Task.Delay(1);
        }
    }
}
