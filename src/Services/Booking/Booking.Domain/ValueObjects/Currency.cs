using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ValueObjects;
public record Currency
{
    public string Code { get; }

    private Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Currency code cannot be empty");
        Code = code.ToUpperInvariant();
    }

    public static Currency FromCode(string code) => new(code);
    public static Currency USD => new("USD");
    public static Currency EUR => new("EUR");

    public override string ToString() => Code;
}