using FlightSearch.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Events;
public record BookingConfirmedDomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Entities.Booking Booking { get; }

    public BookingConfirmedDomainEvent(Entities.Booking booking) => Booking = booking;
}