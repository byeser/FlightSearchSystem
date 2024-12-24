using FlightSearch.Shared.Domain;

namespace Booking.Domain.Events;
public record BookingCreatedDomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Entities.Booking Booking { get; }

    public BookingCreatedDomainEvent(Entities.Booking booking) => Booking = booking;
}