using System.Globalization;

namespace System;
public struct Money
{
    public Money(decimal amount)
    {
        Validation.BasedOn(errors =>
            {
                if(amount < 0m)
                    errors.Add("Amount cannot be negative");
            }
        );
        Amount = amount;
    }

    public Money(double amount) : this(Convert.ToDecimal(amount))
    {
    }


    public Money(uint amount) : this(amount / 100m) { }

    public Money(int amount) : this(amount / 100m) { }

    public override string ToString()
    {
        return Amount.ToString("F2");
    }

    public string ToString(CultureInfo culture)
    {
        return Amount.ToString("C2", culture);
    }

    private decimal Amount { get; set; }
    public decimal Pence => Amount * 100m;

    public static bool operator ==(Money @this, Money that)
    {
        return @this.Equals(that);
    }

    public static bool operator !=(Money @this, Money that)
    {
        return !(@this == that);
    }

    public static Money operator -(Money @this, Money that)
    {
        return new Money(@this.Amount - that.Amount);
    }

    public static Money operator +(Money @this, Money that)
    {
        return new Money(@this.Amount + that.Amount);
    }

    public static implicit operator decimal(Money money)
    {
        return money.Amount;
    }

    public static implicit operator Money(decimal amount)
    {
        return new Money(amount);
    }

    public static implicit operator uint(Money money)
    {
        return Convert.ToUInt32(money.Pence);
    }

    public static implicit operator Money(uint amount)
    {
        return new Money(amount);
    }

    public static implicit operator Money(int amount)
    {
        return new Money(amount);
    }

    public bool Equals(Money that)
    {
        return Amount == that.Amount;
    }

    public override bool Equals(object? that)
    {
        return that is Money money && Equals(money);
    }

    public override int GetHashCode()
    {
        return Amount.GetHashCode();
    }
}