using System;
using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    [TestFixture]
    public partial class MoneySpecs : Specification
    {
        [Test]
        public void provides_currency_amount_formatted_to_4dp()
        {
            Given(a_monetary_amount);
            When(getting_amounts);
            Then(currency_is_in_pounds);
            And(formatted_to_two_decimal_places);
        }

        [Test]
        public void has_value_to_4dp()
        {
            Given(a_monetary_amount);
            When(getting_amounts);
            Then(its_value_is_same_as_monetary_amount);
        }


        [Test]
        public void pence_returns_total_number_of_pence()
        {
            Given(a_monetary_amount);
            When(getting_amounts);
            Then(pence_has_total_pence);
        }

        [Test]
        public void can_be_use_arithmetically_subtraction()
        {
            Given(a_monetary_amount);
            When(subtracting);
            Then(result_is_the_difference_of_the_monetary_amounts);
        }

        [Test]
        public void can_be_use_arithmetically_addition()
        {
            Given(a_monetary_amount);
            When(adding);
            Then(result_is_the_sum_of_the_monetary_amounts);
        }

        [Test]
        public void can_be_created_from_pence()
        {
            Given(a_monetary_amount_in_pence);
            When(getting_amounts);
            Then(its_value_is_same_as_monetary_pence);
        }

        [Test]
        public void negative_monetary_cannot_be_used()
        {
            Given(a_negative_monetary_amount);
            When(getting_amounts);
            Then(() => informs<ValidationException>("Amount cannot be negative"));
        }
    }
}