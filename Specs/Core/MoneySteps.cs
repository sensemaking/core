using System;
using System.Globalization;
using System.Linq;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    public partial class MoneySpecs
    {
        private static readonly CultureInfo chosen_currency = new("en-GB");
        private decimal monetary_amount;
        private uint monetary_pence;
        private string string_amount;
        private string currency_amount;
        private decimal value_from_amount;
        private decimal value_from_pence;
        private decimal pence;
        private uint implicitpence;
        private Money calculated;

        protected override void before_each()
        {
            base.before_each();
            monetary_amount = 0m;
            monetary_pence = 0;
            string_amount = string.Empty;
            currency_amount = string.Empty;
            value_from_amount = 0m;
            pence = 0;
            implicitpence = 0;
            calculated = 0;
        }

        private void a_monetary_amount()
        {
            monetary_amount = 13.42646m;
        }

        private void a_monetary_amount_in_pence()
        {
            a_monetary_amount();
            monetary_pence = 1124;
        }

        private void a_negative_monetary_amount()
        {
            monetary_amount = -0.001m;
        }

        private void getting_amounts()
        {
            trying(() =>
            {
                string_amount = new Money(monetary_amount).ToString();
                currency_amount = new Money(monetary_amount).ToString(chosen_currency);
                Money money = monetary_amount;
                value_from_amount = money;
                value_from_pence = new Money(monetary_pence).Pence;
                pence = money.Pence;
                implicitpence = money;
            });
        }

        private void subtracting()
        {
            calculated = new Money(monetary_amount + 1.6745m) - new Money(monetary_amount);
        }

        private void adding()
        {
            calculated = new Money(monetary_amount) + new Money(monetary_amount);
        }

        private void it_is_formatted_to_2dp()
        {
            decimal.Parse(string_amount).should_be(Math.Round(monetary_amount, 2));
        }

        private void can_be_in_chosen_currency()
        {
            currency_amount.Substring(0, 1).should_be("£");
        }

        private void its_value_is_same_as_monetary_amount()
        {
            value_from_amount.should_be(monetary_amount);
        }

        private void its_value_is_same_as_monetary_pence()
        {
            value_from_pence.should_be(Convert.ToDecimal(monetary_pence));
        }

        private void pence_has_total_pence()
        {
            pence.should_be(1342.646m);
            implicitpence.should_be(1343);
        }

        private void result_is_the_sum_of_the_monetary_amounts()
        {
            ((decimal) calculated).should_be(monetary_amount * 2);
        }

        private void result_is_the_difference_of_the_monetary_amounts()
        {
            ((decimal) calculated).should_be(1.6745m);
        }
    }
}