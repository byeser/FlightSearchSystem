using Booking.Domain.Enums;
using Booking.Domain.Events;
using Booking.Domain.ValueObjects; 
using FlightSearch.Shared.Domain;
using FlightSearch.Shared.Exceptions;


namespace Booking.Domain.Entities;
public class Booking : IAggregateRoot
{
    private readonly List<Passenger> _passengers = new();
    private readonly List<IDomainEvent> _domainEvents = new();

    public BookingId Id { get; private set; }
    public FlightReference FlightReference { get; private set; }
    public IReadOnlyCollection<Passenger> Passengers => _passengers.AsReadOnly();
    public BookingStatus Status { get; private set; }
    public Money TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Booking() { }

    public static Booking Create(
        FlightReference flightReference,
        IEnumerable<Passenger> passengers,
        Money totalAmount)
    {
        var booking = new Booking
        {
            Id = BookingId.Create(),
            FlightReference = flightReference,
            Status = BookingStatus.Created,
            TotalAmount = totalAmount,
            CreatedAt = DateTime.UtcNow
        };

        booking._passengers.AddRange(passengers);
        booking._domainEvents.Add(new BookingCreatedDomainEvent(booking));

        return booking;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Created)
            throw new DomainException("Booking can only be confirmed when in Created status");

        Status = BookingStatus.Confirmed;
        _domainEvents.Add(new BookingConfirmedDomainEvent(this));
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            throw new DomainException("Booking is already cancelled");

        Status = BookingStatus.Cancelled;
        _domainEvents.Add(new BookingCancelledDomainEvent(this));
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}