using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ValueObjects;

public record BookingId
{
    public Guid Value { get; }

    private BookingId(Guid value) => Value = value;

    public static BookingId Create() => new(Guid.NewGuid());
    public static BookingId FromGuid(Guid guid) => new(guid);
}
