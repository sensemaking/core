using System;
using NUnit.Framework;


namespace Sensemaking.Bdd
{
    public abstract class Specification
    {
        protected Exception the_exception;

        [OneTimeSetUp]
        public void TestFixtureSetup() { before_all(); }

        [SetUp]
        public void Setup() { before_each(); }

        [TearDown]
        public void TearDown() { after_each(); }

        [OneTimeTearDown]
        public void TestFixtureTearDown() { after_all(); }

        protected virtual void before_all() { }

        protected virtual void before_each()
        {
            the_exception = null;
        }

        public void Given(Action action) { action.Invoke(); }

        public void When(Action action) { action.Invoke(); }

        public void Then(Action action) { action.Invoke(); }

        public void And(Action action) { action.Invoke(); }

        protected virtual void trying(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                the_exception = e is AggregateException ? e.InnerException : e;
            }
        }

        protected virtual Action informs<T>(string message) where T : Exception
        {
            return () =>
            {
                if (the_exception == null)
                    "it".should_fail("Exception was not provided.");

                if (the_exception is not T)
                    "it".should_fail($"Exception was {the_exception.GetType()} but should have been {typeof(T)}.");

                if (message.IsNullOrEmpty())
                    return;

                if (the_exception is not IListExceptionErrors exceptionWithErrors)
                    the_exception!.Message.should_be(message);
                else
                    exceptionWithErrors.Errors.should_contain(message);
            };
        }

        protected virtual void informs<T>() where T : Exception
        {
           informs<T>(null)();
        }

        protected virtual void causes<T>() where T : Exception
        {
            if (the_exception == null)
                "it".should_fail("Exception was not provided.");

            if (the_exception is not T)
                "it".should_fail($"Exception was {the_exception.GetType()} but should have been {typeof(T)}.");
        }

        protected void it_is_valid()
        {
            the_exception.should_be_null();
        }

        protected virtual void after_each() { }

        protected virtual void after_all() { }

        protected void scenario(Action test)
        {
            try
            {
                before_each();
                test();
            }
            finally
            {
                after_each();
            }
        }
    }
}