using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ValueObjects;
public record FlightReference
{
    public string FlightNumber { get; }
    public string Provider { get; }

    private FlightReference(string flightNumber, string provider)
    {
        if (string.IsNullOrWhiteSpace(flightNumber))
            throw new DomainException("Flight number cannot be empty");
        if (string.IsNullOrWhiteSpace(provider))
            throw new DomainException("Provider cannot be empty");

        FlightNumber = flightNumber;
        Provider = provider;
    }

    public static FlightReference Create(string flightNumber, string provider) => new(flightNumber, provider);
}