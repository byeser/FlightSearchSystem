using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ValueObjects;
public record FlightId
{
    public Guid Value { get; }

    private FlightId(Guid value) => Value = value;

    public static FlightId Create() => new(Guid.NewGuid());
    public static FlightId FromGuid(Guid guid) => new(guid);
}