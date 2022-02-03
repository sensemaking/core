using System;
using NUnit.Framework;


namespace Sensemaking.Bdd
{
    public abstract class Specification
    {
        protected Exception the_exception;
        private ValidationException the_validation_exception;

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
            the_validation_exception = null;
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
            catch (ValidationException e)
            {
                the_validation_exception = e;
            }
            catch (AggregateException e)
            {
                if(e.InnerException is ValidationException validationException)
                    the_validation_exception = validationException;
            }
            catch (Exception e)
            {
                the_exception = e is AggregateException ? e.InnerException : e;
            }
        }

        protected virtual void informs(string message)
        {
            if (the_exception == null && the_validation_exception == null)
                "it".should_fail("Exception was not provided.");
            
            the_exception?.Message.should_be(message);
            the_validation_exception?.Errors.should_contain(message);
        }

        protected virtual void informs<T>(string message) where T : Exception
        {
            informs(message);

            if (!(the_exception is T) && !(the_validation_exception is T))
                "it".should_fail($"Exception was {(the_exception != null ? the_exception.GetType() : the_validation_exception.GetType())} but should have been {typeof(T)}.");
        }

        protected void it_is_valid()
        {
            the_exception.should_be_null();
            the_validation_exception.should_be_null();
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