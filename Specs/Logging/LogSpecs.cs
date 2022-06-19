using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    public partial class LogSpecs : Specification
    { 
        [Test]
        public void logs_information()
        {
            Given(a_monitor_info);
            And(a_log);
            When(logging_information);
            Then(the_logging_monitor_is_logged);
            And(the_entry_is_logged);
        }
 
        [Test]
        public void logs_warnings()
        {
            Given(a_monitor_info);
            And(a_log);
            When(logging_warning);
            Then(the_logging_monitor_is_logged);
            And(the_entry_is_logged);
        }

        [Test]
        public void logs_errors()
        {
            Given(a_monitor_info);
            And(a_log);
            When(logging_error);
            Then(the_logging_monitor_is_logged);
            And(the_entry_is_logged);
        }

        [Test]
        public void logs_fatalities()
        {
            Given(a_monitor_info);
            And(a_log);
            When(logging_fatal);
            Then(the_logging_monitor_is_logged);
            And(the_entry_is_logged);
        }
    }
}