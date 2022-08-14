using System;
using NUnit.Framework;


namespace Sensemaking.Bdd
{
    public abstract class Specification
    {
        protected Exception the_exception;
        private IListExceptionErrors the_exception_with_errors;

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
            the_exception_with_errors = null;
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

        protected void validating(Action action)
        {
            try
            {
                action();
            }
            catch (AggregateException e)
            {
                if(e.InnerException is IListExceptionErrors exceptionWIthErrors)
                    the_exception_with_errors = exceptionWIthErrors;
                else
                    the_exception = e.InnerException;
            }
            catch (Exception e)
            {
                if(e is IListExceptionErrors exceptionWithErrors)
                    the_exception_with_errors = exceptionWithErrors;
                else
                    the_exception = e;
            }
        }

        protected virtual void informs(string message)
        {
            if (the_exception == null && the_exception_with_errors == null)
                "it".should_fail("Exception was not provided.");
            
            the_exception?.Message.should_be(message);
            the_exception_with_errors?.Errors.should_contain(message);
        }

        protected virtual void informs<T>(string message) where T : Exception
        {
            informs(message);

            if (!(the_exception is T) && !(the_exception_with_errors is T))
                "it".should_fail($"Exception was {(the_exception != null ? the_exception.GetType() : the_exception_with_errors.GetType())} but should have been {typeof(T)}.");
        }

        protected void it_is_valid()
        {
            the_exception.should_be_null();
            the_exception_with_errors.should_be_null();
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