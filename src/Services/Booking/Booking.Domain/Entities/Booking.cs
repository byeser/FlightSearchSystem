using FlightSearch.Shared.Domain;
using FlightSearch.Domain.ValueObjects;
using FlightSearch.Domain.Entities;
using Booking.Domain.Events;
using Booking.Domain.ValueObjects;
using Booking.Domain.Enums;
using FlightSearch.Shared.Exceptions;

namespace Booking.Domain.Entities;
public class Booking : IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<Passenger> _passengers = new();

    public BookingId Id { get; private set; }
    public FlightId FlightId { get; private set; }
    public IReadOnlyCollection<Passenger> Passengers => _passengers.AsReadOnly();
    public BookingStatus Status { get; private set; }
    public Money TotalPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private Booking() { }

    public static Booking Create(FlightId flightId, IEnumerable<Passenger> passengers, Money totalPrice)
    {
        var booking = new Booking
        {
            Id = BookingId.Create(),
            FlightId = flightId,
            Status = BookingStatus.Created,
            TotalPrice = totalPrice,
            CreatedAt = DateTime.UtcNow
        };

        booking._passengers.AddRange(passengers);
        booking.AddDomainEvent(new BookingCreatedDomainEvent(booking));

        return booking;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Created)
            throw new DomainException("Booking can only be confirmed when in Created status");

        Status = BookingStatus.Confirmed;
        AddDomainEvent(new BookingConfirmedDomainEvent(this));
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            throw new DomainException("Booking is already cancelled");

        Status = BookingStatus.Cancelled;
        AddDomainEvent(new BookingCancelledDomainEvent(this));
    }
} 