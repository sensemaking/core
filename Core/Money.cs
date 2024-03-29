﻿using System.Globalization;

namespace System;
public struct Money
{
    public Money(decimal amount)
    {
        Amount = amount;
    }

    public Money(double amount) : this(Convert.ToDecimal(amount))
    {
    }

    public Money(uint amount) : this(Convert.ToDecimal(amount)) { }

    public Money(int amount) : this(Convert.ToDecimal(amount)) { }

    public Money(long amount) : this(Convert.ToDecimal(amount)) { }

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