using System;
using NUnit.Framework;

namespace Sensemaking.Bdd
{
    public abstract class Specification
    {
        private Exception exception;
        private ValidationException validation_error;

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
            exception = null;
            validation_error = null;
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
                exception = e is AggregateException ? e.InnerException : e;
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
                validation_error = e;
            }
        }

        protected virtual void informs(string message)
        {
            if (exception == null && validation_error == null)
                "it".should_fail("Exception was not provided.");

            if(exception != null)
                exception.Message.should_be(message);
            else
                validation_error.Errors.should_contain(message);
        }

        protected void it_is_valid()
        {
            exception.should_be_null();
            validation_error.should_be_null();
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