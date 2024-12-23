using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Domain.ValueObjects;
public record Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative");

        return new Money(amount, currency);
    }
}