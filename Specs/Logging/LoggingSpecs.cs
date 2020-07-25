using System.Threading;
using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Http.Specs
{
    public partial class LoggingSpecs : Specification
    { 
        [Test]
        public void logs_information_as_json()
        {
            Given(a_log);
            When(logging_information);
            Then(it_logs_as_json);
        }    
 
        [Test]
        public void logs_warning_as_json()
        {
            Given(a_log);
            When(logging_warning);
            Then(it_logs_as_json);
        }

        [Test]
        public void logs_error_as_json()
        {
            Given(a_log);
            When(logging_error);
            Then(it_logs_as_json);
        }

        [Test]
        public void logs_fatal_as_json()
        {
            Given(a_log);
            When(logging_fatal);
            Then(it_logs_as_json);
        }
    }
}