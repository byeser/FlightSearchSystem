using FlightSearch.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Events;
public record BookingCancelledDomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Entities.Booking Booking { get; }

    public BookingCancelledDomainEvent(Entities.Booking booking) => Booking = booking;
}