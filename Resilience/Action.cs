using System;
using System.Threading.Tasks;
using NodaTime;
using Polly;
using Polly.Timeout;

namespace Sensemaking.Resilience
{
    public interface ISpecifyAttemptFrequency
    {
        ISpecifyHowLongToKeepTrying Every(Period frequency);
    }

    public interface ISpecifyHowLongToKeepTrying
    {
        ISpecifyHowToStopTimeout For(Duration duration);
        ISpecifyHowToReportRetry For(int attempts);
    }

    public interface ISpecifyHowToStopTimeout
    {
        ISpecifyHowToReportTimeout UntilThereIsNoException();
    }

    public interface ISpecifyHowToReportTimeout
    {
        IExecutable IfItTimesOut(string timeoutMessage);
        IAsyncExecutable IfItTimesOutAsync(string timeoutMessage);
    }

    public interface ISpecifyHowToReportRetry
    {
        ISpecifyHowToStopRetry IfItFails(Action<Exception> reportRetry);
    }
    
    public interface ISpecifyHowToStopRetry
    {
        IExecutable UntilThereIsNoException();
        IAsyncExecutable UntilThereIsNoExceptionAsync();
    }

    public interface IExecutable
    {
        void Execute();
    }

    public interface IAsyncExecutable : IExecutable
    {
        Task ExecuteAsync();
    }

    public static class Actions
    {
        public static ISpecifyAttemptFrequency KeepTrying(this Action task)
        {
            return new ActionRetry(task);
        }
        public static ISpecifyAttemptFrequency KeepTryingAsync(this Func<Task> task)
        {
            return new AsyncActionRetry(task);
        }
    }

    internal class ActionRetry : ISpecifyAttemptFrequency, ISpecifyHowLongToKeepTrying, ISpecifyHowToStopTimeout, ISpecifyHowToReportTimeout, ISpecifyHowToStopRetry, ISpecifyHowToReportRetry, IExecutable
    {
        private readonly Action task;
        private string timeoutMessage = null!;
        private Duration? duration = null;
        private Period frequency = null!;
        private int? attempts = null;
        private Action<Exception> reportRetry = e => { };

        public ActionRetry(Action task)
        {
            this.task = task;
        }

        public ISpecifyHowLongToKeepTrying Every(Period frequency)
        {
            this.frequency = frequency;
            return this;
        }

        public ISpecifyHowToStopTimeout For(Duration duration)
        {
            this.duration = duration;
            return this;
        }

        public ISpecifyHowToReportRetry For(int attempts)
        {
            this.attempts = attempts;
            return this;
        }

        public ISpecifyHowToReportTimeout UntilThereIsNoException()
        {
            return this;
        }

        public IAsyncExecutable UntilThereIsNoExceptionAsync()
        {
            throw new NotSupportedException();
        }

        IExecutable ISpecifyHowToStopRetry.UntilThereIsNoException()
        {
            return this;
        }

        public IExecutable IfItTimesOut(string timeoutMessage)
        {
            this.timeoutMessage = timeoutMessage;
            return this;
        }

        public IAsyncExecutable IfItTimesOutAsync(string timeoutMessage)
        {
            throw new NotSupportedException();
        }

        public ISpecifyHowToStopRetry IfItFails(Action<Exception> reportRetry)
        {
            this.reportRetry = reportRetry;
            return this;
        }

        public void Execute()
        {
            if (duration.HasValue)
            {
                Action<Context, TimeSpan, Task, Exception> onTimeout = (ctx, span, task, ex) => throw new Exception(timeoutMessage);
                var timeout = Policy.Timeout(duration.Value.ToTimeSpan(), TimeoutStrategy.Pessimistic, onTimeout);
                var retry = Policy.Handle<Exception>().WaitAndRetry(int.MaxValue, i => frequency.ToDuration().ToTimeSpan());
                timeout.Wrap(retry).Execute(task);
            }
            else
            {
                Action<Exception,TimeSpan> onRetry = (exception, span) => reportRetry(exception);
                Policy.Handle<Exception>().WaitAndRetry(attempts!.Value -1, i => frequency.ToDuration().ToTimeSpan(), onRetry).Execute(task);
            }
        }
    }
    internal class AsyncActionRetry : ISpecifyAttemptFrequency, ISpecifyHowLongToKeepTrying, ISpecifyHowToStopTimeout, ISpecifyHowToReportTimeout, ISpecifyHowToStopRetry, ISpecifyHowToReportRetry, IAsyncExecutable
    {
        private readonly Func<Task> task;
        private string timeoutMessage = null!;
        private Duration? duration = null;
        private Period frequency = null!;
        private int? attempts = null;
        private Action<Exception> reportRetry = e => { };

        public AsyncActionRetry(Func<Task> task)
        {
            this.task = task;
        }

        public ISpecifyHowLongToKeepTrying Every(Period frequency)
        {
            this.frequency = frequency;
            return this;
        }

        public ISpecifyHowToStopTimeout For(Duration duration)
        {
            this.duration = duration;
            return this;
        }

        public ISpecifyHowToReportRetry For(int attempts)
        {
            this.attempts = attempts;
            return this;
        }

        public ISpecifyHowToReportTimeout UntilThereIsNoException()
        {
            return this;
        }

        public IAsyncExecutable UntilThereIsNoExceptionAsync()
        {
            return this;
        }

        IExecutable ISpecifyHowToStopRetry.UntilThereIsNoException()
        {
            return this;
        }

        public IExecutable IfItTimesOut(string timeoutMessage)
        {
            return IfItTimesOutAsync(timeoutMessage);
        }

        public IAsyncExecutable IfItTimesOutAsync(string timeoutMessage)
        {
            this.timeoutMessage = timeoutMessage;
            return this;
        }

        public ISpecifyHowToStopRetry IfItFails(Action<Exception> reportRetry)
        {
            this.reportRetry = reportRetry;
            return this;
        }

        public Task ExecuteAsync()
        {
            Action<Exception, TimeSpan> onRetry = (exception, span) => reportRetry(exception);

            var maxAttempts = duration.HasValue
                ? duration.Value.Milliseconds / (int)frequency.Milliseconds
                : attempts!.Value;
            return Policy.Handle<Exception>()
                .WaitAndRetryAsync(maxAttempts - 1, i => frequency.ToDuration().ToTimeSpan(), onRetry)
                .ExecuteAsync(task);
        }

        public void Execute()
        {
            ExecuteAsync().GetAwaiter().GetResult();
        }
    }
}
