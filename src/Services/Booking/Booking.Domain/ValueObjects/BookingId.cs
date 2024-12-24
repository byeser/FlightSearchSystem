using FlightSearch.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ValueObjects;

public record BookingId
{
    public Guid Value { get; }

    private BookingId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("Booking id cannot be empty");
        Value = value;
    }

    public static BookingId Create() => new(Guid.NewGuid());
    public static BookingId FromGuid(Guid guid) => new(guid);
}