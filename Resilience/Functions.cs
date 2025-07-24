using System;
using System.Threading.Tasks;
using NodaTime;
using Polly;
using Polly.Timeout;

namespace Sensemaking.Resilience
{
    public interface ISpecifyAttemptFrequency<T>
    {
        ISpecifyHowLongToKeepTrying<T> Every(Period frequency);
    }

    public interface ISpecifyHowLongToKeepTrying<T>
    {
        ISpecifyHowToStop<T> For(Duration duration);
        ISpecifyHowToReportRetry<T> For(int attempts);
    }

    public interface ISpecifyHowToStop<T>
    {
        ISpecifyHowToReportTimeout<T> Until(Func<T, bool> predicate);
        ISpecifyHowToReportTimeout<T> UntilThereIsNoException();
    }

    public interface ISpecifyHowToReportTimeout<T> : IExecutable<T>
    {
        IExecutable<T> IfItTimesOut(Action mitigatingAction);
        IExecutable<T> IfItTimesOut(Exception timeoutException);
        IExecutable<T> IfItTimesOut(string timeoutExceptionMessage);
    }

    public interface ISpecifyHowToReportRetry<T>
    {
        ISpecifyHowToStopRetry<T> IfItFails(Action<Exception> reportRetry);
    }

    public interface ISpecifyHowToStopRetry<T>
    {
        IExecutable<T> UntilThereIsNoException();
    }

    public interface IExecutable<T>
    {
        T Execute();
    }

    public static class Functions
    {
        public static ISpecifyAttemptFrequency<T> KeepTrying<T>(this Func<T> task)
        {
            return new FunctionRetry<T>(task);
        }
    }

    internal class FunctionRetry<T> : ISpecifyAttemptFrequency<T>, ISpecifyHowLongToKeepTrying<T>, ISpecifyHowToStop<T>, ISpecifyHowToReportTimeout<T>, ISpecifyHowToReportRetry<T>, ISpecifyHowToStopRetry<T>, IExecutable<T>
    {
        private readonly Func<T> task;
        private Func<T, bool>? failureResultCondition;
        private Action timeoutAction;
        private Duration? duration;
        private Period frequency = null!;
        private int? attempts = null;
        private Action<Exception> reportRetry = e => { };

        public FunctionRetry(Func<T> task)
        {
            this.timeoutAction = () => { throw new TimeoutException("Function retry timed out."); };
            this.task = task;
        }

        public ISpecifyHowLongToKeepTrying<T> Every(Period frequency)
        {
            this.frequency = frequency;
            return this;
        }

        public ISpecifyHowToStop<T> For(Duration duration)
        {
            this.duration = duration;
            return this;
        }

        public ISpecifyHowToReportRetry<T> For(int attempts)
        {
            this.attempts = attempts;
            return this;
        }

        public ISpecifyHowToReportTimeout<T> Until(Func<T, bool> successCondition)
        {
            failureResultCondition = result => !successCondition(result);
            return this;
        }

        public ISpecifyHowToReportTimeout<T> UntilThereIsNoException()
        {
            return this;
        }

        public IExecutable<T> IfItTimesOut(Action mitigationAction)
        {
            this.timeoutAction = mitigationAction;
            return this;
        }

        public IExecutable<T> IfItTimesOut(Exception timeoutException)
        {
            this.timeoutAction = () => { throw timeoutException; };
            return this;
        }

        public IExecutable<T> IfItTimesOut(string timeoutExceptionMessage)
        {
            this.timeoutAction = () => { throw new TimeoutException(timeoutExceptionMessage); };
            return this;
        }

        public T Execute()
        {
            if (duration.HasValue)
            {
                Action<Context, TimeSpan, Task, Exception> onTimeout = (ctx, span, task, ex) => this.timeoutAction();

                var timeout = Policy.Timeout(duration.Value.ToTimeSpan(), TimeoutStrategy.Pessimistic, onTimeout);

                var retry = Policy.Handle<Exception>()
                    .OrResult<T>(result => failureResultCondition != null && failureResultCondition(result))
                    .WaitAndRetry(int.MaxValue, i => frequency.ToDuration().ToTimeSpan());

                return timeout.Wrap(retry).Execute(task);
            }
            else
            {
                Action<Exception, TimeSpan> onRetry = (exception, span) => reportRetry(exception);

                return Policy.Handle<Exception>()
                    .WaitAndRetry(attempts!.Value - 1, i => frequency.ToDuration().ToTimeSpan(), onRetry)
                    .Execute(task);
            }
        }

        public ISpecifyHowToStopRetry<T> IfItFails(Action<Exception> reportRetry)
        {
            this.reportRetry = reportRetry;
            return this;
        }

        IExecutable<T> ISpecifyHowToStopRetry<T>.UntilThereIsNoException()
        {
            return this;
        }
    }
}