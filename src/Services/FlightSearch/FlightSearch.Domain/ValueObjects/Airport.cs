using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch.Domain.ValueObjects;
public record Airport
{
    public string Code { get; }
    public string Name { get; }
    public string City { get; }
    public string Country { get; }

    private Airport(string code, string name, string city, string country)
    {
        Code = code;
        Name = name;
        City = city;
        Country = country;
    }

    public static Airport Create(string code, string name, string city, string country)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Airport code cannot be empty");

        return new Airport(code, name, city, country);
    }
}