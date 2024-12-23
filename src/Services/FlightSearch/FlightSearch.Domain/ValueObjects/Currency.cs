using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Domain.ValueObjects;
public record Currency
{
    public string Code { get; }

    private Currency(string code) => Code = code;

    public static Currency USD => new("USD");
    public static Currency EUR => new("EUR"); 
}